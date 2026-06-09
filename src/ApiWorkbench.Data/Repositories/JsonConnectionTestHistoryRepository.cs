using System.Text.Json;
using System.Text.Json.Serialization;
using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Data.Repositories;

public sealed class JsonConnectionTestHistoryRepository : IConnectionTestHistoryRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonConnectionTestHistoryRepository(string filePath)
    {
        _filePath = filePath;

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task<IReadOnlyList<ConnectionTestHistoryItem>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var items = await LoadItemsAsync(cancellationToken);

        return items
            .OrderByDescending(item => item.StartedAt)
            .ToArray();
    }

    public async Task<ConnectionTestHistoryItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var items = await LoadItemsAsync(cancellationToken);

        return items.FirstOrDefault(item => item.Id == id);
    }

    public async Task SaveAsync(
        ConnectionTestHistoryItem item,
        CancellationToken cancellationToken = default)
    {
        var items = await LoadItemsAsync(cancellationToken);
        var existingIndex = items.FindIndex(existing => existing.Id == item.Id);

        if (existingIndex >= 0)
        {
            items[existingIndex] = item;
        }
        else
        {
            items.Add(item);
        }

        await SaveItemsAsync(items, cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var items = await LoadItemsAsync(cancellationToken);
        items.RemoveAll(item => item.Id == id);

        await SaveItemsAsync(items, cancellationToken);
    }

    public async Task ClearAsync(
        CancellationToken cancellationToken = default)
    {
        await SaveItemsAsync(new List<ConnectionTestHistoryItem>(), cancellationToken);
    }

    private async Task<List<ConnectionTestHistoryItem>> LoadItemsAsync(
        CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return new List<ConnectionTestHistoryItem>();
        }

        await using var stream = File.OpenRead(_filePath);

        var items = await JsonSerializer.DeserializeAsync<List<ConnectionTestHistoryItem>>(
            stream,
            _jsonOptions,
            cancellationToken);

        return items ?? new List<ConnectionTestHistoryItem>();
    }

    private async Task SaveItemsAsync(
        List<ConnectionTestHistoryItem> items,
        CancellationToken cancellationToken)
    {
        var directory = Path.GetDirectoryName(_filePath);

        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(_filePath);

        await JsonSerializer.SerializeAsync(
            stream,
            items,
            _jsonOptions,
            cancellationToken);
    }
}
