using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;
using ApiWorkbench.Data.Repositories;

namespace ApiWorkbench.Tests.Data;

public sealed class JsonConnectionProfileRepositoryTests
{
    [Fact]
    public async Task SaveAsync_ThenGetAllAsync_ReturnsSavedProfile()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionProfileRepository(filePath);

        var profile = new ConnectionProfile
        {
            Name = "Example API",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com",
            Description = "Test profile"
        };

        await repository.SaveAsync(profile);

        var profiles = await repository.GetAllAsync();

        Assert.Single(profiles);
        Assert.Equal(profile.Id, profiles[0].Id);
        Assert.Equal("Example API", profiles[0].Name);
        Assert.Equal(ConnectionType.RestApi, profiles[0].ConnectionType);
        Assert.Equal("https://example.com", profiles[0].Target);

        TryDelete(filePath);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingProfile_ReturnsProfile()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionProfileRepository(filePath);

        var profile = new ConnectionProfile
        {
            Name = "Lookup API",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com"
        };

        await repository.SaveAsync(profile);

        var found = await repository.GetByIdAsync(profile.Id);

        Assert.NotNull(found);
        Assert.Equal(profile.Id, found.Id);
        Assert.Equal("Lookup API", found.Name);

        TryDelete(filePath);
    }

    [Fact]
    public async Task SaveAsync_WithExistingId_ReplacesProfile()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionProfileRepository(filePath);

        var id = Guid.NewGuid();

        var original = new ConnectionProfile
        {
            Id = id,
            Name = "Original",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com"
        };

        var updated = new ConnectionProfile
        {
            Id = id,
            Name = "Updated",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.org"
        };

        await repository.SaveAsync(original);
        await repository.SaveAsync(updated);

        var profiles = await repository.GetAllAsync();

        Assert.Single(profiles);
        Assert.Equal("Updated", profiles[0].Name);
        Assert.Equal("https://example.org", profiles[0].Target);

        TryDelete(filePath);
    }

    [Fact]
    public async Task DeleteAsync_RemovesProfile()
    {
        var filePath = CreateTempFilePath();
        var repository = new JsonConnectionProfileRepository(filePath);

        var profile = new ConnectionProfile
        {
            Name = "Delete Me",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com"
        };

        await repository.SaveAsync(profile);
        await repository.DeleteAsync(profile.Id);

        var profiles = await repository.GetAllAsync();

        Assert.Empty(profiles);

        TryDelete(filePath);
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
