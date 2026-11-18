using FlowOps.Application.Interfaces;
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