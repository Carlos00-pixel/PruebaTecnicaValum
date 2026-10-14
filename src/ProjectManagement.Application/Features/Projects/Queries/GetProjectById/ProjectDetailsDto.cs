using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjectById
{
    public record ProjectDetailsDto(
        Guid Id,
        string Name,
        string? Description,
        ProjectStatus Status,
        DateTime CreatedAt,
        Guid OwnerId,
        int TotalTasks,
        int CompletedTasks,
        int InProgressTasks,
        int PendingTasks
    );
}
