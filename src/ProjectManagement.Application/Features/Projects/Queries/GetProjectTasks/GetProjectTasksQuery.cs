using MediatR;
using ProjectManagement.Application.Common;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjectTasks
{
    public record GetProjectTasksQuery(Guid ProjectId, int Page = 1, int PageSize = 10, TaskItemStatus? Status = null, TaskPriority? Priority = null) : IRequest<PagedResult<TaskListItemDto>>;
}
