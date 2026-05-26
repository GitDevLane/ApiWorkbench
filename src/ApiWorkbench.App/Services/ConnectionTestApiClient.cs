using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.App.Services;

public sealed class ConnectionTestApiClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ConnectionTestApiClient(string baseAddress)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseAddress.TrimEnd('/') + "/")
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
    }

    public async Task<ConnectionTestResult> RunMockConnectionTestAsync(
        ConnectionTestRequest request,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/connection-tests/mock",
            request,
            _jsonOptions,
            cancellationToken);

        return await ReadConnectionTestResultAsync(response, cancellationToken);
    }

    public async Task<ConnectionTestResult> RunMockConnectionTestFromProfileAsync(
        ConnectionProfile profile,
        CancellationToken cancellationToken = default)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/connection-tests/mock/profile",
            profile,
            _jsonOptions,
            cancellationToken);

        return await ReadConnectionTestResultAsync(response, cancellationToken);
    }

    private async Task<ConnectionTestResult> ReadConnectionTestResultAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);

            throw new InvalidOperationException(
                $"API request failed with status {(int)response.StatusCode}: {error}");
        }

        var result = await response.Content.ReadFromJsonAsync<ConnectionTestResult>(
            _jsonOptions,
            cancellationToken);

        return result ?? throw new InvalidOperationException("API returned an empty response.");
    }
}
