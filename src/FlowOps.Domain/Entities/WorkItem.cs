using FlowOps.Domain.Enums;

namespace FlowOps.Domain.Entities;

public class WorkItem
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public WorkItemStatus Status { get; private set; }
        = WorkItemStatus.Todo;

    public WorkItemPriority Priority { get; private set; }
        = WorkItemPriority.Medium;

    public DateTime CreatedAt { get; private set; }
        = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; private set; }

    private WorkItem()
    {
        // Required by EF Core
    }

    public WorkItem(
        string title,
        string? description,
        WorkItemPriority priority)
    {
        SetTitle(title);
        Description = description;
        Priority = priority;
    }

    public void Update(
        string title,
        string? description,
        WorkItemPriority priority)
    {
        SetTitle(title);
        Description = description;
        Priority = priority;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(WorkItemStatus status)
    {
        if (!Enum.IsDefined(status))
            throw new ArgumentOutOfRangeException(nameof(status));

        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }

    private void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Work item title is required.",
                nameof(title));

        if (title.Length > 200)
            throw new ArgumentException(
                "Work item title cannot exceed 200 characters.",
                nameof(title));

        Title = title.Trim();
    }
}
