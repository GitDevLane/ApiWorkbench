using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Enums;
using ApiWorkbench.Core.Models;

namespace ApiWorkbench.Infrastructure.Services;

public sealed class ConnectionProfileValidator : IConnectionProfileValidator
{
    public ConnectionProfileValidationResult Validate(ConnectionProfile profile)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(profile.Name))
        {
            errors.Add("Profile name is required.");
        }

        if (profile.ConnectionType == ConnectionType.Unknown)
        {
            errors.Add("Connection type is required.");
        }

        if (string.IsNullOrWhiteSpace(profile.Target))
        {
            errors.Add("Target is required.");
        }

        if ((profile.ConnectionType == ConnectionType.RestApi ||
             profile.ConnectionType == ConnectionType.FastApi) &&
            !IsHttpUrl(profile.Target))
        {
            errors.Add("REST/FastAPI targets must be a valid HTTP or HTTPS URL.");
        }

        return errors.Count == 0
            ? ConnectionProfileValidationResult.Success()
            : ConnectionProfileValidationResult.Failure(errors);
    }

    private static bool IsHttpUrl(string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
        {
            return false;
        }

        return uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps;
    }
}
