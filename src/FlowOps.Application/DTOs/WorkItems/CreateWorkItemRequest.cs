using FlowOps.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FlowOps.Application.DTOs.WorkItems
{
    public sealed class CreateWorkItemRequest
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters.")]
        public string? Description { get; set; }

        [EnumDataType(typeof(WorkItemPriority),
        ErrorMessage = "Invalid priority value.")]
        public WorkItemPriority Priority { get; set; }
            = WorkItemPriority.Medium;
    }
}
