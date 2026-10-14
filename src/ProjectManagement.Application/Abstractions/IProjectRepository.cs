using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Abstractions
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
