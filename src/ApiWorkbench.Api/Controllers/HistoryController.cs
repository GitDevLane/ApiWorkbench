using ApiWorkbench.Core.Abstractions;
using ApiWorkbench.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ApiWorkbench.Api.Controllers;

[ApiController]
[Route("api/history")]
public sealed class HistoryController : ControllerBase
{
    private readonly IConnectionTestHistoryRepository _historyRepository;

    public HistoryController(IConnectionTestHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ConnectionTestHistoryItem>>> GetAll(
        CancellationToken cancellationToken)
    {
        var items = await _historyRepository.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ConnectionTestHistoryItem>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var item = await _historyRepository.GetByIdAsync(id, cancellationToken);

        if (item is null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _historyRepository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Clear(
        CancellationToken cancellationToken)
    {
        await _historyRepository.ClearAsync(cancellationToken);
        return NoContent();
    }
}
