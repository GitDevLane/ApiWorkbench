using ApiWorkbench.Core.Enums;
using ApiWorkbench.Infrastructure.Services;

namespace ApiWorkbench.Tests.Infrastructure;

public sealed class MockConnectionTestServiceTests
{
    [Fact]
    public async Task TestConnectionAsync_ReturnsSuccessfulResult()
    {
        var service = new MockConnectionTestService();

        var result = await service.TestConnectionAsync(
            profileName: "Local Test",
            connectionType: ConnectionType.RestApi,
            target: "https://example.com");

        Assert.True(result.IsSuccess);
        Assert.Equal(ConnectionTestStatus.Success, result.Status);
        Assert.Equal(ConnectionType.RestApi, result.ConnectionType);
        Assert.Equal("Local Test", result.ProfileName);
        Assert.Contains("https://example.com", result.Message);
        Assert.True(result.Duration >= TimeSpan.Zero);
    }
}
