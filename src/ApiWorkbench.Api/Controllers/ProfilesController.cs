using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiWorkbench.Api.Controllers;

[ApiController]
[Route("api/profiles")]
public sealed class ProfilesController : ControllerBase
{
    private readonly IConnectionProfileRepository _profileRepository;
    private readonly IConnectionProfileValidator _profileValidator;

    public ProfilesController(
        IConnectionProfileRepository profileRepository,
        IConnectionProfileValidator profileValidator)
    {
        _profileRepository = profileRepository;
        _profileValidator = profileValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ConnectionProfile>>> GetAll(
        CancellationToken cancellationToken)
    {
        var profiles = await _profileRepository.GetAllAsync(cancellationToken);
        return Ok(profiles);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ConnectionProfile>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var profile = await _profileRepository.GetByIdAsync(id, cancellationToken);

        if (profile is null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Save(
        [FromBody] ConnectionProfile profile,
        CancellationToken cancellationToken)
    {
        var validationResult = _profileValidator.Validate(profile);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        await _profileRepository.SaveAsync(profile, cancellationToken);

        return Ok(profile);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _profileRepository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
