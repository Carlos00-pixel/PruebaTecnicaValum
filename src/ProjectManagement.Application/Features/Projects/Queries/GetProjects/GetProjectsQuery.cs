using MediatR;
using ProjectManagement.Application.Common;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjects
{
    public record GetProjectsQuery(int Page = 1, int PageSize = 10, ProjectStatus? Status = null): IRequest<PagedResult<ProjectListItemDto>>;
}
