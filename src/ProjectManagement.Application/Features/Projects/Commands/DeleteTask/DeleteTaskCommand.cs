using MediatR;

namespace ProjectManagement.Application.Features.Projects.Commands.DeleteTask
{
    public record DeleteTaskCommand(Guid TaskId) : IRequest<Unit>;
}
