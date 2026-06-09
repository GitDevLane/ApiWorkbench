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
        var httpClient = CreateHttpClient(
            HttpStatusCode.OK,
            "OK",
            """{"message":"hello from test"}""",
            "application/json");

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
        Assert.Equal(200, result.HttpStatusCode);
        Assert.Equal("OK", result.ReasonPhrase);
        Assert.Contains("application/json", result.ResponseContentType);
        Assert.Contains("hello from test", result.ResponseBodyPreview);
        Assert.Contains("200", result.Message);
        Assert.Null(result.ErrorMessage);
    }

    [Fact]
    public async Task RunGetAsync_WithFailureResponse_ReturnsFailed()
    {
        var httpClient = CreateHttpClient(
            HttpStatusCode.NotFound,
            "Not Found",
            "Not found",
            "text/plain");

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
        Assert.Equal(404, result.HttpStatusCode);
        Assert.Equal("Not Found", result.ReasonPhrase);
        Assert.Contains("text/plain", result.ResponseContentType);
        Assert.Contains("Not found", result.ResponseBodyPreview);
        Assert.Contains("404", result.Message);
        Assert.NotNull(result.ErrorMessage);
    }

    private static HttpClient CreateHttpClient(
        HttpStatusCode statusCode,
        string reasonPhrase,
        string content,
        string contentType)
    {
        var handler = new FakeHttpMessageHandler(
            statusCode,
            reasonPhrase,
            content,
            contentType);

        return new HttpClient(handler);
    }

    private sealed class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _reasonPhrase;
        private readonly string _content;
        private readonly string _contentType;

        public FakeHttpMessageHandler(
            HttpStatusCode statusCode,
            string reasonPhrase,
            string content,
            string contentType)
        {
            _statusCode = statusCode;
            _reasonPhrase = reasonPhrase;
            _content = content;
            _contentType = contentType;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                ReasonPhrase = _reasonPhrase,
                Content = new StringContent(_content)
            };

            response.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(_contentType);

            return Task.FromResult(response);
        }
    }
}
