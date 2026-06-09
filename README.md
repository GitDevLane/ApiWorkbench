# ApiWorkbench

ApiWorkbench is a portfolio-focused C#/.NET workbench for testing API connections, saving reusable connection profiles, and tracking connection test history.

The MVP demonstrates a WPF desktop frontend connected to an ASP.NET Core Web API backend. It uses shared Core models/contracts, Infrastructure services, JSON-backed storage, and xUnit tests.

## MVP Status

ApiWorkbench MVP v0.1 is complete.

Current working features:

- WPF desktop app
- ASP.NET Core Web API backend
- Shared Core models, enums, and interfaces
- JSON-backed saved profile storage
- JSON-backed connection test history storage
- Save, load, and delete connection profiles
- Run mock connection tests
- Run real REST API GET tests
- Automatically save test results to history
- View and clear history from the WPF app
- Configurable WPF API base URL through appsettings.json
- Unit tests for profile validation, profile storage, history storage, and REST GET testing

## Current Stack

- C#
- .NET 10
- WPF
- ASP.NET Core Web API
- xUnit
- JSON local storage
- PowerShell development workflow

## Solution Structure

ApiWorkbench/
  src/
    ApiWorkbench.App/              WPF desktop UI
    ApiWorkbench.Api/              ASP.NET Core Web API
    ApiWorkbench.Core/             Shared models, enums, interfaces
    ApiWorkbench.Infrastructure/   Connection test services and validation
    ApiWorkbench.Data/             JSON-backed repositories
  tests/
    ApiWorkbench.Tests/            xUnit tests
  docs/                            Project documentation
  samples/                         Sample profile files
  scripts/                         Future helper scripts

## Current End-to-End Flow

WPF App
  -> builds or loads a ConnectionProfile
  -> calls ASP.NET Core API
  -> API validates the profile
  -> API runs a mock or real REST GET connection test
  -> API saves the result to history
  -> WPF displays the result and history

## Build and Test

From the repo root:

dotnet test .\ApiWorkbench.slnx
dotnet build .\ApiWorkbench.slnx

## Run the API

From the repo root:

dotnet run --project .\src\ApiWorkbench.Api\ApiWorkbench.Api.csproj --urls "http://localhost:5075"

## Run the WPF App

Open a second PowerShell window:

dotnet run --project .\src\ApiWorkbench.App\ApiWorkbench.App.csproj

## Manual Demo Flow

1. Start the API.
2. Start the WPF app.
3. Click Load Profiles.
4. Create or select a RestApi profile.
5. Use target https://example.com.
6. Click Save Profile.
7. Click Run REST GET.
8. Confirm the result shows Success and Error: None.
9. Confirm the history list updates.
10. Select a history item to view details.
11. Optionally clear history.

## Sample Profile

A sample REST API profile is included at:

samples/connection-profiles/example-rest-api-profile.json

This sample points to https://example.com and can be used to demonstrate a basic REST GET test.

## MVP Notes

This MVP intentionally avoids storing secrets, tokens, passwords, or real connection strings.

Future versions may add:

- SQL Server connection testing
- PostgreSQL connection testing
- API headers and authentication options
- Result export
- Better WPF layout and MVVM refactor
- FastAPI/AWS expansion
- Encrypted local secret storage
