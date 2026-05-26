using ApiWorkbench.Core.Enums;

namespace ApiWorkbench.Api.Requests;

public sealed class ConnectionTestRequest
{
    public string ProfileName { get; init; } = string.Empty;

    public ConnectionType ConnectionType { get; init; } = ConnectionType.Unknown;

    public string Target { get; init; } = string.Empty;
}
