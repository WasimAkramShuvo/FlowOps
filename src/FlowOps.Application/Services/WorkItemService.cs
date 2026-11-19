using FlowOps.Application.DTOs.WorkItems;
using FlowOps.Application.Interfaces;
using FlowOps.Application.Mappings;
using FlowOps.Domain.Entities;
using FlowOps.Domain.Enums;

namespace FlowOps.Application.Services
{
    public class WorkItemService
    {
        private readonly IWorkItemRepository _repository;

        public WorkItemService(IWorkItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<WorkItemResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = await _repository.GetByIdAsync(
                id,
                cancellationToken);

            return item?.ToResponse();
        }

        public async Task<WorkItemResponse> CreateAsync(CreateWorkItemRequest request, CancellationToken cancellationToken = default)
        {
            var workItem = new WorkItem(
                request.Title,
                request.Description,
                request.Priority);

            await _repository.AddAsync(
                workItem,
                cancellationToken);

            await _repository.SaveChangesAsync(
                cancellationToken);

            return workItem.ToResponse();
        }

        public async Task<bool> UpdateAsync(Guid id, string title, string description, WorkItemPriority priority, CancellationToken cancellationToken = default)
        {
            var workItem = await _repository.GetByIdAsync(
                id, cancellationToken);

            if (workItem is null)
                return false;

            workItem.Update(title, description, priority);

            _repository.Update(workItem);

            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> ChangeStatusAsync(
            Guid id,
            WorkItemStatus status,
            CancellationToken cancellationToken = default)
        {
            var workItem = await _repository.GetByIdAsync(
                id,
                cancellationToken);

            if (workItem is null)
                return false;

            workItem.ChangeStatus(status);

            _repository.Update(workItem);

            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var workItem = await _repository.GetByIdAsync(
                id,
                cancellationToken);

            if (workItem is null)
                return false;

            _repository.Delete(workItem);

            await _repository.SaveChangesAsync(
                cancellationToken);

            return true;
        }

    }
}
