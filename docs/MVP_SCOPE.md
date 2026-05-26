# MVP Scope

The first MVP should stay focused and practical.

The goal is not to build a full Postman clone or a full database administration tool. The goal is to build a clean, portfolio-ready workbench that proves API, database, and integration testing skills.

## MVP Goal

Build a C#/.NET workbench that can:

- Start from a WPF desktop app
- Call an ASP.NET Core Web API
- Test a connection through a service layer
- Return a structured result
- Display the result to the user
- Log or save test results later

## Phase 1A - Buildable Baseline

Completed:

- Created GitHub repo structure
- Created .NET solution
- Added WPF app project
- Added ASP.NET Core Web API project
- Added Core class library
- Added Infrastructure class library
- Added Data class library
- Added xUnit test project
- Connected project references
- Confirmed solution builds successfully

## Phase 1B - Shared Core Models

Completed:

- Added `ConnectionType`
- Added `ConnectionTestStatus`
- Added `ConnectionTestResult`
- Added `ConnectionTestRequest`
- Added `IConnectionTestService`

## Phase 1C - Mock Service

Completed:

- Added `MockConnectionTestService`
- Added test coverage for mock service
- Confirmed tests pass

## Phase 1D - Web API Endpoint

Completed:

- Added `ConnectionTestsController`
- Added `POST /api/connection-tests/mock`
- Registered `IConnectionTestService`
- Confirmed endpoint works from PowerShell

## Phase 1E - WPF to API Flow

Completed:

- Added WPF API client
- Added basic WPF test UI
- WPF app calls API endpoint
- WPF app displays returned `ConnectionTestResult`

## Phase 1F - Documentation

In progress:

- Update README
- Document current architecture
- Document MVP scope
- Document current workflows
- Document security notes

## Recommended Next Features

1. Improve the WPF layout and result panel
2. Add API base URL configuration
3. Add connection profile model
4. Add local profile storage
5. Add real REST API test service
6. Add SQL Server connection test service
7. Add result history
8. Add screenshots and usage examples
