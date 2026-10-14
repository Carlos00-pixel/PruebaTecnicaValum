using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjects
{
    public record ProjectListItemDto(Guid Id, string Name, string? Description, ProjectStatus Status, DateTime CreatedAt, Guid OwnerId);
}