# MVP Scope

The first ApiWorkbench MVP is focused on a practical, portfolio-ready C#/.NET workbench.

The goal is not to build a full Postman clone or a full database administration tool. The goal is to demonstrate a clean frontend/backend architecture with reusable connection profiles, real REST API testing, and result history.

## MVP Goal

Build a C#/.NET workbench that can:

- Start from a WPF desktop app
- Call an ASP.NET Core Web API
- Save and load connection profiles
- Validate connection profiles
- Run mock connection tests
- Run real REST API GET tests
- Save test results to history
- Display results and history in the WPF app

## Completed MVP Features

Completed:

- GitHub repo structure
- Buildable .NET solution
- WPF app project
- ASP.NET Core Web API project
- Core class library
- Infrastructure class library
- Data class library
- xUnit test project
- Shared connection test models
- Shared connection profile models
- Profile validation
- Mock connection test service
- Profile-based mock API endpoint
- WPF-to-API mock test flow
- WPF appsettings configuration
- JSON profile storage
- Profile API endpoints
- WPF save/load/delete profile workflow
- JSON history storage
- History API endpoints
- Automatic history saving after tests
- WPF history viewer
- WPF clear history workflow
- Real REST API GET test service
- REST GET API endpoint
- WPF REST GET workflow
- Sample REST API profile
- MVP documentation

## MVP Finish Criteria

The MVP is considered complete when:

- dotnet test succeeds
- dotnet build succeeds
- API starts on http://localhost:5075
- WPF app starts
- A RestApi profile can be saved
- A saved profile can be loaded
- A saved profile can be deleted
- A real REST GET test can be run against https://example.com
- The result shows Success and Error: None
- The history grid updates after the test
- History can be selected and cleared

## Future Work Outside MVP

Future features may include:

- SQL Server connection testing
- PostgreSQL connection testing
- REST request headers
- REST request body support
- API authentication options
- Result export
- Better WPF styling
- MVVM refactor
- Database-backed storage
- FastAPI testing
- AWS testing
- Encrypted secret storage
