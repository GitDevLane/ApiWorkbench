using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Infrastructure.Services;

public sealed class RestApiConnectionTestService : IRestApiConnectionTestService
{
    private const int MaxResponsePreviewChars = 4000;

    private readonly HttpClient _httpClient;

    public RestApiConnectionTestService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.Timeout = TimeSpan.FromSeconds(15);
    }

    public async Task<ConnectionTestResult> RunGetAsync(
        ConnectionProfile profile,
        CancellationToken cancellationToken = default)
    {
        var startedAt = DateTimeOffset.UtcNow;

        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, profile.Target);
            request.Headers.UserAgent.ParseAdd("ApiWorkbench/1.0");

            using var response = await _httpClient.SendAsync(
                request,
                HttpCompletionOption.ResponseHeadersRead,
                cancellationToken);

            var bodyPreview = await ReadBodyPreviewAsync(
                response.Content,
                cancellationToken);

            var completedAt = DateTimeOffset.UtcNow;
            var isSuccess = response.IsSuccessStatusCode;

            return new ConnectionTestResult
            {
                ProfileName = profile.Name,
                ConnectionType = profile.ConnectionType,
                Status = isSuccess ? ConnectionTestStatus.Success : ConnectionTestStatus.Failed,
                Message = $"HTTP GET completed with status {(int)response.StatusCode} {response.ReasonPhrase}.",
                ErrorMessage = isSuccess
                    ? null
                    : $"HTTP request returned non-success status code {(int)response.StatusCode}.",
                HttpStatusCode = (int)response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                ResponseContentType = response.Content.Headers.ContentType?.ToString(),
                ResponseBodyPreview = bodyPreview,
                StartedAt = startedAt,
                CompletedAt = completedAt
            };
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            var completedAt = DateTimeOffset.UtcNow;

            return new ConnectionTestResult
            {
                ProfileName = profile.Name,
                ConnectionType = profile.ConnectionType,
                Status = ConnectionTestStatus.Failed,
                Message = "HTTP GET request timed out.",
                ErrorMessage = ex.Message,
                StartedAt = startedAt,
                CompletedAt = completedAt
            };
        }
        catch (Exception ex)
        {
            var completedAt = DateTimeOffset.UtcNow;

            return new ConnectionTestResult
            {
                ProfileName = profile.Name,
                ConnectionType = profile.ConnectionType,
                Status = ConnectionTestStatus.Failed,
                Message = "HTTP GET request failed.",
                ErrorMessage = ex.Message,
                StartedAt = startedAt,
                CompletedAt = completedAt
            };
        }
    }

    private static async Task<string?> ReadBodyPreviewAsync(
        HttpContent content,
        CancellationToken cancellationToken)
    {
        var body = await content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(body))
        {
            return null;
        }

        if (body.Length <= MaxResponsePreviewChars)
        {
            return body;
        }

        return body[..MaxResponsePreviewChars]
            + Environment.NewLine
            + "... [response preview truncated]";
    }
}
