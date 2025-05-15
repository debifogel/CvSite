using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Octokit;
using System.Collections.ObjectModel;

namespace CvSite.Service
{
    public class GitHubPortfolioCachingDecorator : IGitHubPortfolioService
    {
        private readonly IGitHubPortfolioService _inner;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "GitHubPortfolio";
        private const string LastFetchTimeKey = "LastFetchTime";

        public GitHubPortfolioCachingDecorator(IGitHubPortfolioService inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public async Task<IReadOnlyList<Octokit.Repository>> GetRepositoriesAsync()
        {
            // Check for cached repositories
            if (_cache.TryGetValue(CacheKey, out IReadOnlyList<Repository>? cachedRepos)
                && _cache.TryGetValue(LastFetchTimeKey, out DateTime lastFetchTime))
            {
                // Check if the GitHub repositories have been updated since last fetch
                var freshData = await _inner.GetRepositoriesAsync();
                DateTime latestUpdateTime = freshData.Max(repo => repo.PushedAt?.DateTime ?? DateTime.MinValue);

                // If the latest update time is later than the last fetch time, refresh the cache
                if (latestUpdateTime > lastFetchTime)
                {
                    _cache.Set(CacheKey, freshData);
                    _cache.Set(LastFetchTimeKey, DateTime.Now);
                    return freshData;
                }

                return (IReadOnlyList<Octokit.Repository>)cachedRepos; // Return cached value
            }

            // No cached data, fetch fresh data and cache it
            var newData = await _inner.GetRepositoriesAsync();
            _cache.Set(CacheKey, newData);
            _cache.Set(LastFetchTimeKey, DateTime.Now);
            return newData;
        }

        public async Task<IReadOnlyList<Octokit.Repository>> SearchRepositoriesAsync(string? name, string? language, string? user)
        {
            return await _inner.SearchRepositoriesAsync(name, language, user);
        }
    }
}