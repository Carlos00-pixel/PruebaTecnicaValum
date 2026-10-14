using MediatR;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Commands.CreateTask
{
    public record CreateTaskCommand(Guid ProjectId, string Title, string? Description, TaskPriority Priority, DateTime? DueDate) : IRequest<Guid>;
}
