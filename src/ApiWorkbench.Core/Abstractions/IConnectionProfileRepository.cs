using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Core.Abstractions;

public interface IConnectionProfileRepository
{
    Task<IReadOnlyList<ConnectionProfile>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ConnectionProfile?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        ConnectionProfile profile,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
