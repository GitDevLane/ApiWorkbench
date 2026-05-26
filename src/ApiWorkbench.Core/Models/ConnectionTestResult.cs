using ApiWorkbench.Core.Enums;

namespace ApiWorkbench.Core.Models;

public sealed class ConnectionTestResult
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string ProfileName { get; init; } = string.Empty;

    public ConnectionType ConnectionType { get; init; } = ConnectionType.Unknown;

    public ConnectionTestStatus Status { get; init; } = ConnectionTestStatus.Unknown;

    public string Message { get; init; } = string.Empty;

    public string? ErrorMessage { get; init; }

    public DateTimeOffset StartedAt { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset CompletedAt { get; init; } = DateTimeOffset.UtcNow;

    public TimeSpan Duration => CompletedAt - StartedAt;

    public bool IsSuccess => Status == ConnectionTestStatus.Success;
}
