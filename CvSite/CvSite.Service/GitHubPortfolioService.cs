using Microsoft.Extensions.Options;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CvSite.Service
{
    public class GitHubPortfolioService: IGitHubPortfolioService
    {
        private readonly GitHubClient _client;
        private readonly string _userName;

        public GitHubPortfolioService(IOptions<GitHubOptions> options)
        {
            _userName = options.Value.UserName;
            _client = new GitHubClient(new ProductHeaderValue("DevPortfolioApp"))
            {
                Credentials = new Credentials(options.Value.Token)
            };
        }

        public async Task<IReadOnlyList<Octokit.Repository>> GetRepositoriesAsync()
        {
            return (IReadOnlyList<Octokit.Repository>)await _client.Repository.GetAllForUser(_userName);
        }

        public async Task<IReadOnlyList<Octokit.Repository>> SearchRepositoriesAsync(string? name, string? language, string? user)
        {
            var request = new SearchRepositoriesRequest(name)
            {
                Language = !string.IsNullOrWhiteSpace(language) && Enum.TryParse(language, true, out Language parsedLanguage)
            ? parsedLanguage
            : null,
                User = user
            };

            var result = await _client.Search.SearchRepo(request);
            return (IReadOnlyList<Octokit.Repository>)result.Items;
        }

    }

}
