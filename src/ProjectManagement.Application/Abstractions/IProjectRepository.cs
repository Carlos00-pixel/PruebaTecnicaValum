using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Abstractions
{
    public interface IProjectRepository
    {
        Task AddAsync(Project project, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<(IReadOnlyList<Project> Items, int TotalCount)> GetPagedAsync(ProjectStatus? status, int page, int pageSize, CancellationToken cancellationToken);
    }
}
