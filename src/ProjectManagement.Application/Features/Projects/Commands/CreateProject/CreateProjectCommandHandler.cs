using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandHandler: IRequestHandler<CreateProjectCommand, Guid>
    {
        private readonly IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project
            {
                OwnerId = request.OwnerId,
                Name = request.Name.Trim(),
                Description = request.Description?.Trim()
            };

            await _projectRepository.AddAsync(project, cancellationToken);

            await _projectRepository.SaveChangesAsync(cancellationToken);

            return project.Id;
        }
    }
}
