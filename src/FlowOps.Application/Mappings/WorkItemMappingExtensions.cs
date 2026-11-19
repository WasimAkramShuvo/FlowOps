using FlowOps.Application.DTOs.WorkItems;
using FlowOps.Domain.Entities;

namespace FlowOps.Application.Mappings
{
    public static class WorkItemMappingExtensions
    {
        public static WorkItemResponse ToResponse(
            this WorkItem workItem)
        {
            return new WorkItemResponse
            {
                Id = workItem.Id,
                Title = workItem.Title,
                Description = workItem.Description,
                Status = workItem.Status,
                Priority = workItem.Priority,
                CreatedAt = workItem.CreatedAt,
                UpdatedAt = workItem.UpdatedAt
            };
        }
    }
}
