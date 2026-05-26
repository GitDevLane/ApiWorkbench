# ApiWorkbench

ApiWorkbench is a portfolio-focused C#/.NET workbench for testing API connections, database connections, and integration workflows.

The project starts with a C#/.NET stack and is designed to expand later into PostgreSQL, FastAPI, AWS, and reusable project templates.

## Current Status

Phase 1 is in progress.

Currently working:

- Buildable .NET solution
- WPF desktop app
- ASP.NET Core Web API
- Shared Core models and interfaces
- Mock connection test service
- WPF app can call the API and display a connection test result

## Current Stack

- C#
- .NET 10
- WPF
- ASP.NET Core Web API
- xUnit
- PowerShell development workflow

## Current Solution Structure

```text
ApiWorkbench/
├── src/
│   ├── ApiWorkbench.App/
│   ├── ApiWorkbench.Api/
│   ├── ApiWorkbench.Core/
│   ├── ApiWorkbench.Infrastructure/
│   └── ApiWorkbench.Data/
├── tests/
│   └── ApiWorkbench.Tests/
├── docs/
├── samples/
├── scripts/
├── ApiWorkbench.slnx
├── README.md
└── LICENSE
