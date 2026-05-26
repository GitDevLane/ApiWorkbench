using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiWorkbench.Api.Controllers;

[ApiController]
[Route("api/connection-tests")]
public sealed class ConnectionTestsController : ControllerBase
{
    private readonly IConnectionTestService _connectionTestService;
    private readonly IConnectionProfileValidator _profileValidator;

    public ConnectionTestsController(
        IConnectionTestService connectionTestService,
        IConnectionProfileValidator profileValidator)
    {
        _connectionTestService = connectionTestService;
        _profileValidator = profileValidator;
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

    [HttpPost("mock/profile")]
    public async Task<IActionResult> RunMockConnectionTestFromProfile(
        [FromBody] ConnectionProfile profile,
        CancellationToken cancellationToken)
    {
        var validationResult = _profileValidator.Validate(profile);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var result = await _connectionTestService.TestConnectionAsync(
            profile.Name,
            profile.ConnectionType,
            profile.Target,
            cancellationToken);

        return Ok(result);
    }
}
