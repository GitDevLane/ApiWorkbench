using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;
using ApiWorkbench.Data.Repositories;

namespace ApiWorkbench.Tests.Data;

public sealed class JsonConnectionTestHistoryRepositoryTests
{
    [Fact]
    public async Task SaveAsync_ThenGetAllAsync_ReturnsSavedHistoryItem()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionTestHistoryRepository(filePath);

        var item = CreateHistoryItem("Example API");

        await repository.SaveAsync(item);

        var items = await repository.GetAllAsync();

        Assert.Single(items);
        Assert.Equal(item.Id, items[0].Id);
        Assert.Equal("Example API", items[0].ProfileName);
        Assert.Equal(ConnectionType.RestApi, items[0].ConnectionType);
        Assert.Equal(ConnectionTestStatus.Success, items[0].Status);
        Assert.True(items[0].IsSuccess);

        TryDelete(filePath);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingItem_ReturnsHistoryItem()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionTestHistoryRepository(filePath);

        var item = CreateHistoryItem("Lookup API");

        await repository.SaveAsync(item);

        var found = await repository.GetByIdAsync(item.Id);

        Assert.NotNull(found);
        Assert.Equal(item.Id, found.Id);
        Assert.Equal("Lookup API", found.ProfileName);

        TryDelete(filePath);
    }

    [Fact]
    public async Task DeleteAsync_RemovesHistoryItem()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionTestHistoryRepository(filePath);

        var item = CreateHistoryItem("Delete API");

        await repository.SaveAsync(item);
        await repository.DeleteAsync(item.Id);

        var items = await repository.GetAllAsync();

        Assert.Empty(items);

        TryDelete(filePath);
    }

    [Fact]
    public async Task ClearAsync_RemovesAllHistoryItems()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionTestHistoryRepository(filePath);

        await repository.SaveAsync(CreateHistoryItem("API One"));
        await repository.SaveAsync(CreateHistoryItem("API Two"));

        await repository.ClearAsync();

        var items = await repository.GetAllAsync();

        Assert.Empty(items);

        TryDelete(filePath);
    }

    private static ConnectionTestHistoryItem CreateHistoryItem(string profileName)
    {
        var startedAt = DateTimeOffset.UtcNow;
        var completedAt = startedAt.AddMilliseconds(250);

        return new ConnectionTestHistoryItem
        {
            ProfileName = profileName,
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com",
            Status = ConnectionTestStatus.Success,
            Message = "Mock connection test succeeded.",
            StartedAt = startedAt,
            CompletedAt = completedAt
        };
    }

    private static string CreateTempFilePath()
    {
        return Path.Combine(
            Path.GetTempPath(),
            "ApiWorkbenchTests",
            $"{Guid.NewGuid()}.json");
    }

    private static void TryDelete(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
