using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Infrastructure.Services;

public sealed class MockConnectionTestService : IConnectionTestService
{
    public async Task<ConnectionTestResult> TestConnectionAsync(
        string profileName,
        ConnectionType connectionType,
        string target,
        CancellationToken cancellationToken = default)
    {
        var startedAt = DateTimeOffset.UtcNow;

        await Task.Delay(250, cancellationToken);

        var completedAt = DateTimeOffset.UtcNow;

        return new ConnectionTestResult
        {
            ProfileName = profileName,
            ConnectionType = connectionType,
            Status = ConnectionTestStatus.Success,
            Message = $"Mock connection test succeeded for target: {target}",
            StartedAt = startedAt,
            CompletedAt = completedAt
        };
    }
}
