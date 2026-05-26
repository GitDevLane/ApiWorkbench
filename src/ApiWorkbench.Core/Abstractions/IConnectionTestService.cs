using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Core.Abstractions;

public interface IConnectionTestService
{
    Task<ConnectionTestResult> TestConnectionAsync(
        string profileName,
        ConnectionType connectionType,
        string target,
        CancellationToken cancellationToken = default);
}
