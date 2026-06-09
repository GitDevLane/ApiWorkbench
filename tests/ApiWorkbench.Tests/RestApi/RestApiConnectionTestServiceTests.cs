using System.Net;
using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;
using ApiWorkbench.Infrastructure.Services;

namespace ApiWorkbench.Tests.RestApi;

public sealed class RestApiConnectionTestServiceTests
{
    [Fact]
    public async Task RunGetAsync_WithSuccessfulResponse_ReturnsSuccess()
    {
        var httpClient = CreateHttpClient(HttpStatusCode.OK, "OK");
        var service = new RestApiConnectionTestService(httpClient);

        var profile = new ConnectionProfile
        {
            Name = "Example API",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com"
        };

        var result = await service.RunGetAsync(profile);

        Assert.True(result.IsSuccess);
        Assert.Equal(ConnectionTestStatus.Success, result.Status);
        Assert.Equal(ConnectionType.RestApi, result.ConnectionType);
        Assert.Equal("Example API", result.ProfileName);
        Assert.Contains("200", result.Message);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task RunGetAsync_WithFailureResponse_ReturnsFailed()
    {
        var httpClient = CreateHttpClient(HttpStatusCode.NotFound, "Not Found");
        var service = new RestApiConnectionTestService(httpClient);

        var profile = new ConnectionProfile
        {
            Name = "Missing API",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com/missing"
        };

        var result = await service.RunGetAsync(profile);

        Assert.False(result.IsSuccess);
        Assert.Equal(ConnectionTestStatus.Failed, result.Status);
        Assert.Contains("404", result.Message);
        Assert.NotNull(result.ErrorMessage);
    }

    private static HttpClient CreateHttpClient(HttpStatusCode statusCode, string reasonPhrase)
    {
        var handler = new FakeHttpMessageHandler(statusCode, reasonPhrase);

        return new HttpClient(handler);
    }

    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _reasonPhrase;

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string reasonPhrase)
        {
            _statusCode = statusCode;
            _reasonPhrase = reasonPhrase;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                ReasonPhrase = _reasonPhrase,
                Content = new StringContent(string.Empty)
            };

            return Task.FromResult(response);
        }
    }
}
