using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Core.Abstractions;

public interface IConnectionTestHistoryRepository
{
    Task<IReadOnlyList<ConnectionTestHistoryItem>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ConnectionTestHistoryItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        ConnectionTestHistoryItem item,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task ClearAsync(
        CancellationToken cancellationToken = default);
}
