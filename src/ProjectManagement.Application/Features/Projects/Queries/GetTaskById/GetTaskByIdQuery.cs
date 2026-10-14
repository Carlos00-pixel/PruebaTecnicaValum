using MediatR;

namespace ProjectManagement.Application.Features.Projects.Queries.GetTaskById
{
    public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDetailsDto>;
}
