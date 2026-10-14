using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQueryHandler: IRequestHandler<GetProjectByIdQuery, ProjectDetailsDto>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectByIdQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<ProjectDetailsDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id, cancellationToken);

            if(project is null)
            {
                throw new NotFoundException($"No se ha encontrado el projecto con el identificado: {request.Id}.");
            }

            var tasks = project.Tasks;

            var totalTasks = tasks.Count;

            var completedTasks = tasks.Count(task => task.Status == TaskItemStatus.Done);
            var inProgressTasks = tasks.Count(task => task.Status == TaskItemStatus.InProgress);
            var pendingTasks = tasks.Count(task => task.Status == TaskItemStatus.Todo);

            return new ProjectDetailsDto(
                project.Id,
                project.Name,
                project.Description,
                project.Status,
                project.CreatedAt,
                project.OwnerId,
                totalTasks,
                completedTasks,
                inProgressTasks,
                pendingTasks);
        }
    }
}
