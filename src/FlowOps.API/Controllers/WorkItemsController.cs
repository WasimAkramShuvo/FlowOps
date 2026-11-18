
using FlowOps.Application.Interfaces;
using FlowOps.Domain.Entities;
using FlowOps.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlowOps.API.Controllers;

[ApiController]
[Route("api/workitems")]
public class WorkItemsController : ControllerBase
{
    private readonly IWorkItemRepository _repository;

    public WorkItemsController(IWorkItemRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);

        if (item is null)
            return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateWorkItemRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Title)
            || request.Title.Length > 200
            || (request.Description?.Length ?? 0) > 2000
            || !Enum.IsDefined(request.Priority))
        {
            return BadRequest("Invalid work item data.");
        }

        var item = new WorkItem(
            request.Title,
            request.Description,
            request.Priority);

        await _repository.AddAsync(item, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = item.Id },
            item);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateWorkItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);

        if (item is null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(request.Title)
            || request.Title.Length > 200
            || (request.Description?.Length ?? 0) > 2000
            || !Enum.IsDefined(request.Priority))
        {
            return BadRequest("Invalid work item data.");
        }

        item.Update(
            request.Title,
            request.Description,
            request.Priority);

        _repository.Update(item);
        await _repository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeWorkItemStatusRequest request,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);

        if (item is null)
            return NotFound();

        if (!Enum.IsDefined(request.Status))
            return BadRequest("Invalid status.");

        item.ChangeStatus(request.Status);

        _repository.Update(item);
        await _repository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);

        if (item is null)
            return NotFound();

        _repository.Delete(item);
        await _repository.SaveChangesAsync(cancellationToken);

        return NoContent();
    }
}

public record CreateWorkItemRequest(
    string Title,
    string? Description,
    WorkItemPriority Priority);

public record UpdateWorkItemRequest(
    string Title,
    string? Description,
    WorkItemPriority Priority);

public record ChangeWorkItemStatusRequest(
    WorkItemStatus Status);
