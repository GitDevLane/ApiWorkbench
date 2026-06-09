# Current Handoff

## Project

ApiWorkbench

## MVP Status

ApiWorkbench MVP v0.1 is complete.

The project has a working C#/.NET WPF frontend, ASP.NET Core Web API backend, shared Core contracts, Infrastructure services, JSON-backed profile storage, JSON-backed history storage, and xUnit tests.

## Current Working Features

- WPF app runs
- ASP.NET Core API runs on http://localhost:5075
- WPF app can call the API
- Profiles can be saved, loaded, and deleted
- Mock connection tests work
- Real REST API GET tests work
- Test results are saved to history
- History can be loaded, viewed, selected, and cleared
- API base URL is configurable in the WPF appsettings.json
- Sample REST API profile exists in samples/connection-profiles

## Current End-to-End Flow

WPF App
  -> creates or loads ConnectionProfile
  -> ConnectionTestApiClient
  -> ASP.NET Core API
  -> ConnectionTestsController
  -> IConnectionProfileValidator
  -> MockConnectionTestService or RestApiConnectionTestService
  -> JsonConnectionTestHistoryRepository
  -> ConnectionTestResult
  -> WPF result display and history grid

## Main Projects

src/ApiWorkbench.App
    WPF desktop app

src/ApiWorkbench.Api
    ASP.NET Core Web API

src/ApiWorkbench.Core
    Shared models, enums, and interfaces

src/ApiWorkbench.Infrastructure
    Service implementations and validators

src/ApiWorkbench.Data
    JSON-backed repositories

tests/ApiWorkbench.Tests
    xUnit tests

## Important Models and Services

ConnectionProfile
ConnectionTestRequest
ConnectionTestResult
ConnectionTestHistoryItem
ConnectionProfileValidationResult
ConnectionType
ConnectionTestStatus
IConnectionTestService
IRestApiConnectionTestService
IConnectionProfileValidator
IConnectionProfileRepository
IConnectionTestHistoryRepository
MockConnectionTestService
RestApiConnectionTestService
ConnectionProfileValidator
JsonConnectionProfileRepository
JsonConnectionTestHistoryRepository
ConnectionTestApiClient
ConnectionTestsController
ProfilesController
HistoryController

## Build and Test

From repo root:

dotnet test .\ApiWorkbench.slnx
dotnet build .\ApiWorkbench.slnx

## Run API

dotnet run --project .\src\ApiWorkbench.Api\ApiWorkbench.Api.csproj --urls "http://localhost:5075"

## Run WPF App

In a second PowerShell window:

dotnet run --project .\src\ApiWorkbench.App\ApiWorkbench.App.csproj

## Last Known Good Manual Test

1. Start API on http://localhost:5075.
2. Start WPF app.
3. Load profiles.
4. Select or create a RestApi profile with target https://example.com.
5. Save profile.
6. Run REST GET.
7. Confirm result shows Success, HTTP 200 OK, Error: None, and Is Success: True.
8. Confirm history updates.
9. Select a history item and confirm details display.
10. Clear history and confirm list clears.

## Known Limitations

- WPF UI is functional but still basic.
- The app is not fully MVVM structured yet.
- REST GET currently supports simple unauthenticated GET requests.
- Profiles are stored in JSON, not a database.
- History is stored in JSON, not a database.
- Secrets, tokens, passwords, and private connection strings should not be stored in this MVP.

## Recommended Future Phases

1. Polish WPF layout and spacing.
2. Add screenshots to README.
3. Add SQL Server connection test.
4. Add REST API headers/auth support.
5. Add result export.
6. Add MVVM refactor.
7. Add PostgreSQL support.
8. Add FastAPI/AWS expansion.
