using FlowOps.Domain.Enums;

namespace FlowOps.Application.DTOs.WorkItems
{
    public sealed class ChangeWorkItemStatusRequest
    {
        public WorkItemStatus Status { get; set; }
    }
}
