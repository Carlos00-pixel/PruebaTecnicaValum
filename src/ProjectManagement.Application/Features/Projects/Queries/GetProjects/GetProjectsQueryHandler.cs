using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Common;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjects
{
    public class GetProjectsQueryHandler: IRequestHandler<GetProjectsQuery, PagedResult<ProjectListItemDto>>
    {
        private const int DefaultPage = 1;
        private const int DefaultPageSize = 10;
        private const int MaxPageSize = 50;

        private readonly IProjectRepository _projectRepository;

        public GetProjectsQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<PagedResult<ProjectListItemDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var page = Math.Max(request.Page, DefaultPage);

            var pageSize = request.PageSize <= 0 ? DefaultPageSize : Math.Min(request.PageSize, MaxPageSize);

            var (projects, totalCount) = await _projectRepository.GetPagedAsync(request.Status, page, pageSize, cancellationToken);

            var items = projects.Select(project => new ProjectListItemDto(project.Id, project.Name, project.Description, project.Status, project.CreatedAt, project.OwnerId)).ToList();

            return new PagedResult<ProjectListItemDto>(items, page, pageSize, totalCount);
        }
    }
}
