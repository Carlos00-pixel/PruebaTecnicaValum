using MediatR;

namespace ProjectManagement.Application.Features.Projects.Commands.CreateProject
{
    public record CreateProjectCommand(Guid OwnerId, string Name, string? Description): IRequest<Guid>;
}
