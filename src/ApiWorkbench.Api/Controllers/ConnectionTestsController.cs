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
    private readonly IConnectionTestHistoryRepository _historyRepository;

    public ConnectionTestsController(
        IConnectionTestService connectionTestService,
        IConnectionProfileValidator profileValidator,
        IConnectionTestHistoryRepository historyRepository)
    {
        _connectionTestService = connectionTestService;
        _profileValidator = profileValidator;
        _historyRepository = historyRepository;
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

        await _historyRepository.SaveAsync(
            ToHistoryItem(result, request.Target),
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

        await _historyRepository.SaveAsync(
            ToHistoryItem(result, profile.Target),
            cancellationToken);

        return Ok(result);
    }

    private static ConnectionTestHistoryItem ToHistoryItem(
        ConnectionTestResult result,
        string target)
    {
        return new ConnectionTestHistoryItem
        {
            ProfileName = result.ProfileName,
            ConnectionType = result.ConnectionType,
            Target = target,
            Status = result.Status,
            Message = result.Message,
            ErrorMessage = result.ErrorMessage,
            StartedAt = result.StartedAt,
            CompletedAt = result.CompletedAt
        };
    }
}
