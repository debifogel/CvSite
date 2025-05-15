using Octokit;

public interface IGitHubPortfolioService
{
   Task<IReadOnlyList<Octokit.Repository>> GetRepositoriesAsync();
   Task<IReadOnlyList<Octokit.Repository>> SearchRepositoriesAsync(string? name, string? language, string? user);

}
public class Repository
{
    public string Name { get; set; }
    public string Language { get; set; }
    public int StargazersCount { get; set; }
    public string HtmlUrl { get; set; }
    public DateTimeOffset? PushedAt { get; set; }
}