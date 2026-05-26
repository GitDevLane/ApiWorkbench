# Project Overview

ApiWorkbench is a developer/admin workbench for testing API connections, database connections, and integration workflows.

The goal is to build a practical portfolio project that demonstrates real-world skills in:

- C#/.NET development
- WPF desktop UI development
- ASP.NET Core Web API development
- Shared library architecture
- Dependency injection
- HTTP/API communication
- Database connection testing
- Logging and audit-style reporting
- Safe handling of connection information
- Future cloud/API expansion

## Purpose

Many business systems depend on connections between applications, APIs, databases, and cloud services. ApiWorkbench is designed to provide a controlled interface where a user can test and inspect those connections.

Examples:

- Test whether a SQL Server connection works
- Test whether a REST API endpoint responds
- View response status, timing, and errors
- Save reusable connection profiles
- Track test history
- Generate simple troubleshooting reports

## Current Architecture

```text
ApiWorkbench.App
    WPF frontend used by the user

ApiWorkbench.Api
    ASP.NET Core Web API layer

ApiWorkbench.Core
    Shared models, enums, and service contracts

ApiWorkbench.Infrastructure
    Service implementations such as connection testers

ApiWorkbench.Data
    Future persistence layer for profiles, logs, and settings

ApiWorkbench.Tests
    Automated tests

