using CvSite.Service; // Ensure this namespace contains IGitHubPortfolioService
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DevPortfolio.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private readonly IGitHubPortfolioService _service;

        public PortfolioController(IGitHubPortfolioService service)
        {
            _service = service;
        }

        [HttpGet("GetPortfolio")]
        public async Task<IActionResult> GetPortfolio()
        {
            var repos = await _service.GetRepositoriesAsync();
            return Ok(repos.Select(repo => new
            {
                repo.Name,
                repo.Language,
                repo.StargazersCount,
                repo.HtmlUrl,
                LastCommit = repo.PushedAt?.DateTime // Ensure PushedAt exists
            }).ToList());
        }

        [HttpGet("SearchRepositories")]
        public async Task<IActionResult> SearchRepositories(string? name, string? language, string? user)
        {
            var results = await _service.SearchRepositoriesAsync(name, language, user);
            return Ok(results.Select(repo => new
            {
                repo.Name,
                repo.HtmlUrl,
                repo.Language,
                repo.StargazersCount
            }).ToList());
        }
    }
}
