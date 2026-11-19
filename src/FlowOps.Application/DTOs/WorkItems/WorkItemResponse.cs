using FlowOps.Domain.Enums;

namespace FlowOps.Application.DTOs.WorkItems
{
    public sealed class WorkItemResponse
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public WorkItemStatus Status { get; set; }

        public WorkItemPriority Priority { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
