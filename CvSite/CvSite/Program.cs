using CvSite.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<GitHubOptions>(
    builder.Configuration.GetSection("GitHub"));

builder.Services.AddMemoryCache();

// הרשמת GitHubPortfolioService + דקורטור
builder.Services.AddScoped<GitHubPortfolioService>();
builder.Services.AddScoped<IGitHubPortfolioService, GitHubPortfolioService>();
builder.Services.Decorate<IGitHubPortfolioService, GitHubPortfolioCachingDecorator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
