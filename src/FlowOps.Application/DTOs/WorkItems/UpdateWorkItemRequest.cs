using FlowOps.Domain.Enums;

namespace FlowOps.Application.DTOs.WorkItems
{
    public sealed class UpdateWorkItemRequest
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public WorkItemPriority Priority { get; set; }
            = WorkItemPriority.Medium;
    }
}
