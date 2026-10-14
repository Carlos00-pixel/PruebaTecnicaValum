using MediatR;

namespace ProjectManagement.Application.Features.Projects.Commands.ArchiveProject
{
    public record ArchiveProjectCommand(Guid ProjectId): IRequest<Unit>;
}
