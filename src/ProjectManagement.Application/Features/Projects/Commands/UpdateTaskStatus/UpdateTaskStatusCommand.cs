using MediatR;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Commands.UpdateTaskStatus
{
    public record UpdateTaskStatusCommand(Guid TaskId, TaskItemStatus Status): IRequest<Unit>;
}
