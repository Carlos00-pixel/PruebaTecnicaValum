using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Abstractions
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem project, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
