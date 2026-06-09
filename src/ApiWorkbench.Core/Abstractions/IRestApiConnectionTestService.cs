using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Core.Abstractions;

public interface IRestApiConnectionTestService
{
    Task<ConnectionTestResult> RunGetAsync(
        ConnectionProfile profile,
        CancellationToken cancellationToken = default);
}
