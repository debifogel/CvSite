# CvSite

## API Overview: PortfolioController

The `PortfolioController` is an ASP.NET Core API controller for interacting with GitHub repositories. It provides endpoints to list and search repositories using the `IGitHubPortfolioService`.

### Features

1. **Get Portfolio**
   - **Endpoint**: `GET /api/Portfolio/GetPortfolio`
   - **Description**: Fetches a list of repositories managed by the service.

2. **Search Repositories**
   - **Endpoint**: `GET /api/Portfolio/SearchRepositories`
   - **Description**: Allows searching for repositories based on optional filters.
   - **Parameters**:
     - `name` (string, optional): Filter results by repository name.
     - `language` (string, optional): Filter results by programming language.
     - `user` (string, optional): Filter results by the repository owner's username.

