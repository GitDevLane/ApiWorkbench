using System.Text.Json;
using System.Text.Json.Serialization;
using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Data.Repositories;

public sealed class JsonConnectionProfileRepository : IConnectionProfileRepository
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    public JsonConnectionProfileRepository(string filePath)
    {
        _filePath = filePath;

        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true
        };

        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task<IReadOnlyList<ConnectionProfile>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await LoadProfilesAsync(cancellationToken);
    }

    public async Task<ConnectionProfile?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var profiles = await LoadProfilesAsync(cancellationToken);

        return profiles.FirstOrDefault(profile => profile.Id == id);
    }

    public async Task SaveAsync(
        ConnectionProfile profile,
        CancellationToken cancellationToken = default)
    {
        var profiles = await LoadProfilesAsync(cancellationToken);
        var existingIndex = profiles.FindIndex(existing => existing.Id == profile.Id);

        if (existingIndex >= 0)
        {
            profiles[existingIndex] = profile;
        }
        else
        {
            profiles.Add(profile);
        }

        await SaveProfilesAsync(profiles, cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var profiles = await LoadProfilesAsync(cancellationToken);
        profiles.RemoveAll(profile => profile.Id == id);

        await SaveProfilesAsync(profiles, cancellationToken);
    }

    private async Task<List<ConnectionProfile>> LoadProfilesAsync(
        CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return new List<ConnectionProfile>();
        }

        await using var stream = File.OpenRead(_filePath);

        var profiles = await JsonSerializer.DeserializeAsync<List<ConnectionProfile>>(
            stream,
            _jsonOptions,
            cancellationToken);

        return profiles ?? new List<ConnectionProfile>();
    }

    private async Task SaveProfilesAsync(
        List<ConnectionProfile> profiles,
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
            profiles,
            _jsonOptions,
            cancellationToken);
    }
}
