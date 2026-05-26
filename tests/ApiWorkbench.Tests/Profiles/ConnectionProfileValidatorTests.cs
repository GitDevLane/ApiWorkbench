using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;
using ApiWorkbench.Infrastructure.Services;

namespace ApiWorkbench.Tests.Profiles;

public sealed class ConnectionProfileValidatorTests
{
    [Fact]
    public void Validate_WithValidRestApiProfile_ReturnsValid()
    {
        var validator = new ConnectionProfileValidator();

        var profile = new ConnectionProfile
        {
            Name = "Example API",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com"
        };

        var result = validator.Validate(profile);

        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WithMissingName_ReturnsInvalid()
    {
        var validator = new ConnectionProfileValidator();

        var profile = new ConnectionProfile
        {
            Name = "",
            ConnectionType = ConnectionType.RestApi,
            Target = "https://example.com"
        };

        var result = validator.Validate(profile);

        Assert.False(result.IsValid);
        Assert.Contains("Profile name is required.", result.Errors);
    }

    [Fact]
    public void Validate_WithUnknownConnectionType_ReturnsInvalid()
    {
        var validator = new ConnectionProfileValidator();

        var profile = new ConnectionProfile
        {
            Name = "Unknown Type",
            ConnectionType = ConnectionType.Unknown,
            Target = "https://example.com"
        };

        var result = validator.Validate(profile);

        Assert.False(result.IsValid);
        Assert.Contains("Connection type is required.", result.Errors);
    }

    [Fact]
    public void Validate_WithInvalidRestApiTarget_ReturnsInvalid()
    {
        var validator = new ConnectionProfileValidator();

        var profile = new ConnectionProfile
        {
            Name = "Bad API",
            ConnectionType = ConnectionType.RestApi,
            Target = "not-a-url"
        };

        var result = validator.Validate(profile);

        Assert.False(result.IsValid);
        Assert.Contains("REST/FastAPI targets must be a valid HTTP or HTTPS URL.", result.Errors);
    }
}
