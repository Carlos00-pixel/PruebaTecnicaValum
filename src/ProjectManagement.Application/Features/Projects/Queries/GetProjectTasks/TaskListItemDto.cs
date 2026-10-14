using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjectTasks
{
    public record TaskListItemDto(Guid Id, Guid ProjectId, string Title, string? Description, TaskPriority Priority, TaskItemStatus Status, DateTime? DueDate, DateTime CreatedAt, DateTime? CompletedAt);
}
