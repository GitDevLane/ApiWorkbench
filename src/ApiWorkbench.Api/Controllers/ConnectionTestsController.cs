using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiWorkbench.Api.Controllers;

[ApiController]
[Route("api/connection-tests")]
public sealed class ConnectionTestsController : ControllerBase
{
    private readonly IConnectionTestService _connectionTestService;

    public ConnectionTestsController(IConnectionTestService connectionTestService)
    {
        _connectionTestService = connectionTestService;
    }

    [HttpPost("mock")]
    public async Task<IActionResult> RunMockConnectionTest(
        [FromBody] ConnectionTestRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ProfileName))
        {
            return BadRequest("ProfileName is required.");
        }

        if (string.IsNullOrWhiteSpace(request.Target))
        {
            return BadRequest("Target is required.");
        }

        var result = await _connectionTestService.TestConnectionAsync(
            request.ProfileName,
            request.ConnectionType,
            request.Target,
            cancellationToken);

        return Ok(result);
    }
}
