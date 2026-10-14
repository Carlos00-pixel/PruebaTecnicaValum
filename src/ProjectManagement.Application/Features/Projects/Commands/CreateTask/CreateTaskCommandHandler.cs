using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Commands.CreateTask
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        public CreateTaskCommandHandler(IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
        }

        public async Task<Guid> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

            if(project is null)
            {
                throw new NotFoundException($"No se ha encontrado el prooyecto con el identificador: {request.ProjectId}.");
            }

            if(project.Status == ProjectStatus.Archived)
            {
                throw new DomainException("No se pueden crear tareas en un proyecto archivado.");
            }

            var task = new TaskItem
            {
                ProjectId = request.ProjectId,
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                Priority = request.Priority,
                DueDate = request.DueDate,
                Status = TaskItemStatus.Todo,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = null,
                IsDeleted = false
            };

            await _taskRepository.AddAsync(task, cancellationToken);

            await _taskRepository.SaveChangesAsync(cancellationToken);

            return task.Id;
        }
    }
}
