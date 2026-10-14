using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Features.Projects.Commands.ArchiveProject
{
    public class ArchiveProjectCommandHandler: IRequestHandler<ArchiveProjectCommand, Unit>
    {
        private readonly IProjectRepository _projectRepository;

        public ArchiveProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(ArchiveProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

            if (project is null)
            {
                throw new NotFoundException($"No se ha encontrado el proyecto con el identificador: {request.ProjectId}.");
            }

            var hasTasksInProgress = project.Tasks.Any(task => task.Status == TaskItemStatus.InProgress);

            if (hasTasksInProgress)
            {
                throw new DomainException("No se puede archivar un proyecto que tiene tareas en curso.");
            }

            project.Status = ProjectStatus.Archived;

            await _projectRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
