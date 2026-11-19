using FlowOps.Application.Common;
using FlowOps.Application.Interfaces;
using FlowOps.Application.Queries;
using FlowOps.Domain.Entities;
using FlowOps.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FlowOps.Infrastructure.Repositories;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly FlowOpsDbContext _context;

    public WorkItemRepository(FlowOpsDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<WorkItem>> GetPagedAsync(
    WorkItemQueryParameters parameters,
    CancellationToken cancellationToken = default)
    {
        if (parameters.PageNumber < 1)
            throw new ArgumentException(
                "Page number must be greater than zero.");

        if (parameters.PageSize < 1)
            throw new ArgumentException(
                "Page size must be greater than zero.");

        IQueryable<WorkItem> query = _context.WorkItems
            .AsNoTracking();

        // Filtering
        if (parameters.Status.HasValue)
        {
            if (!Enum.IsDefined(parameters.Status.Value))
                throw new ArgumentException("Invalid status value.");

            query = query.Where(x =>
                x.Status == parameters.Status.Value);
        }

        if (parameters.Priority.HasValue)
        {
            if (!Enum.IsDefined(parameters.Priority.Value))
                throw new ArgumentException("Invalid priority value.");

            query = query.Where(x =>
                x.Priority == parameters.Priority.Value);
        }

        // Count filtered records before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Sorting with deterministic ID tie-breaker
        query = (parameters.SortBy?.ToLowerInvariant(),
                 parameters.SortDescending) switch
        {
            ("title", false) => query
                .OrderBy(x => x.Title).ThenBy(x => x.Id),

            ("title", true) => query
                .OrderByDescending(x => x.Title).ThenBy(x => x.Id),

            ("priority", false) => query
                .OrderBy(x => x.Priority).ThenBy(x => x.Id),

            ("priority", true) => query
                .OrderByDescending(x => x.Priority).ThenBy(x => x.Id),

            ("createdat", false) => query
                .OrderBy(x => x.CreatedAt).ThenBy(x => x.Id),

            ("createdat", true) => query
                .OrderByDescending(x => x.CreatedAt).ThenBy(x => x.Id),

            _ => throw new ArgumentException(
                "SortBy must be Title, Priority, or CreatedAt.")
        };

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedResult<WorkItem>
        {
            Items = items,
            PageNumber = parameters.PageNumber,
            PageSize = parameters.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<List<WorkItem>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.WorkItems
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task AddAsync(
        WorkItem workItem,
        CancellationToken cancellationToken = default)
    {
        await _context.WorkItems.AddAsync(
            workItem,
            cancellationToken);
    }

    public void Update(WorkItem workItem)
    {
        _context.WorkItems.Update(workItem);
    }

    public void Delete(WorkItem workItem)
    {
        _context.WorkItems.Remove(workItem);
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}