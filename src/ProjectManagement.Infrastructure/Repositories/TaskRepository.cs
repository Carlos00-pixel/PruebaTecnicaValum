using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;
using ProjectManagement.Infrastructure.Persistence;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TaskItem task, CancellationToken cancellationToken)
        {
            await _dbContext.Tasks.AddAsync(task, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Tasks.AsNoTracking().FirstOrDefaultAsync(task => task.Id == id, cancellationToken);
        }

        public async Task<(IReadOnlyList<TaskItem> Items, int TotalCount)> GetPagedAsync(Guid projectId, TaskItemStatus? status, TaskPriority? priority, int page, int pageSize, CancellationToken cancellationToken)
        {
            var query = _dbContext.Tasks.AsNoTracking().Where(t => t.ProjectId == projectId);

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (priority.HasValue)
            {
                query = query.Where(t => t.Priority == priority.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query.OrderByDescending(t => t.CreatedAt).ThenBy(t => t.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
