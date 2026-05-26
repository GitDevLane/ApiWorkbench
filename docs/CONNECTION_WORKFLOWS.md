# Connection Workflows

This document describes the current and planned connection testing workflows for ApiWorkbench.

## Current Mock Connection Workflow

The current working workflow is a mock test used to prove the architecture.

```text
User opens WPF app
User enters:
    Profile Name
    Connection Type
    Target

User clicks Run Mock Test

WPF sends POST request to:
    /api/connection-tests/mock

API validates:
    ProfileName is not blank
    Target is not blank

API calls:
    IConnectionTestService.TestConnectionAsync()

Infrastructure returns:
    ConnectionTestResult

WPF displays:
    Id
    Profile Name
    Connection Type
    Status
    Message
    Started At
    Completed At
    Duration
    Is Success
