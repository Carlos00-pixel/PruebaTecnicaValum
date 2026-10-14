using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Abstractions
{
    public interface ITaskRepository
    {
        Task AddAsync(TaskItem project, CancellationToken cancellationToken);

        Task SaveChangesAsync(CancellationToken cancellationToken);

        Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedAsync(Guid projectId, TaskItemStatus? status, TaskPriority? priority, int page, int pageSize, CancellationToken cancellationToken);
    }
}
