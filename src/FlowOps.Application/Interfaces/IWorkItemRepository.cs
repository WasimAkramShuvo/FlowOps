using FlowOps.Application.Common;
using FlowOps.Application.Queries;
using FlowOps.Domain.Entities;

namespace FlowOps.Application.Interfaces;

public interface IWorkItemRepository
{
    Task<List<WorkItem>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<WorkItem?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        WorkItem workItem,
        CancellationToken cancellationToken = default);

    void Update(WorkItem workItem);

    void Delete(WorkItem workItem);

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task<PaginatedResult<WorkItem>> GetPagedAsync(
    WorkItemQueryParameters parameters,
    CancellationToken cancellationToken = default);
}