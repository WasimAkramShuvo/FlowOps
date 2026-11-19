
using FlowOps.Application.Common;
using FlowOps.Application.DTOs.WorkItems;
using FlowOps.Application.Interfaces;
using FlowOps.Application.Mappings;
using FlowOps.Application.Queries;
using FlowOps.Domain.Entities;
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

    [HttpGet("paged")]
    [ProducesResponseType(
    typeof(PaginatedResult<WorkItem>),
    StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedResult<WorkItem>>> GetPaged(
    [FromQuery] WorkItemQueryParameters parameters,
    CancellationToken cancellationToken)
    {
        if (parameters.PageNumber < 1)
        {
            return BadRequest(new
            {
                message = "PageNumber must be greater than zero."
            });
        }

        if (parameters.Status.HasValue &&
            !Enum.IsDefined(parameters.Status.Value))
        {
            return BadRequest(new
            {
                message = "Invalid status value."
            });
        }

        if (parameters.Priority.HasValue &&
            !Enum.IsDefined(parameters.Priority.Value))
        {
            return BadRequest(new
            {
                message = "Invalid priority value."
            });
        }

        var result = await _repository.GetPagedAsync(
            parameters,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<WorkItemResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllAsync(cancellationToken);

        var response = items.Select(item => item.ToResponse()).ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WorkItemResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(id, cancellationToken);

        if (item is null)
            return NotFound();

        return Ok(item.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<WorkItemResponse>> Create(
        [FromBody] CreateWorkItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = new WorkItem(
            request.Title,
            request.Description,
            request.Priority);

        await _repository.AddAsync(
            item, cancellationToken);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = item.Id },
            item.ToResponse());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateWorkItemRequest request,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(
            id, cancellationToken);

        if (item is null)
            return NotFound();

        item.Update(
            request.Title,
            request.Description,
            request.Priority);

        _repository.Update(item);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] ChangeWorkItemStatusRequest request,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(
            id, cancellationToken);

        if (item is null)
            return NotFound();

        item.ChangeStatus(request.Status);

        _repository.Update(item);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var item = await _repository.GetByIdAsync(
            id, cancellationToken);

        if (item is null)
            return NotFound();

        _repository.Delete(item);

        await _repository.SaveChangesAsync(
            cancellationToken);

        return NoContent();
    }
}