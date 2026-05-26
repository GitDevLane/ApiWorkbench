using ApiWorkbench.Core.Enums;

namespace ApiWorkbench.Core.Models;

public sealed class ConnectionProfile
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; init; } = string.Empty;

    public ConnectionType ConnectionType { get; init; } = ConnectionType.Unknown;

    public string Target { get; init; } = string.Empty;

    public string? Description { get; init; }

    public bool IsActive { get; init; } = true;

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}
