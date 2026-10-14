using MediatR;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjectById
{
    public record GetProjectByIdQuery(Guid Id): IRequest<ProjectDetailsDto>;
}
