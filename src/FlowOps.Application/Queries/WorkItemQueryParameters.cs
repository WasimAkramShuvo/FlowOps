using FlowOps.Domain.Enums;

namespace FlowOps.Application.Queries
{
    public sealed class WorkItemQueryParameters
    {
        private const int MaxPageSize = 100;

        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Clamp(value, 1, MaxPageSize);
        }

        public WorkItemStatus? Status { get; set; }

        public WorkItemPriority? Priority { get; set; }

        public string SortBy { get; set; } = "CreatedAt";

        public bool SortDescending { get; set; } = true;
    }
}
