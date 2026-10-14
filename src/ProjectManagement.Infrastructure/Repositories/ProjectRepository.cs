using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;
using ProjectManagement.Infrastructure.Persistence;

namespace ProjectManagement.Infrastructure.Repositories
{
    public class ProjectRepository: IProjectRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProjectRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            await _dbContext.Projects.AddAsync(project, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<Project?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Projects.Include(project => project.Tasks).FirstOrDefaultAsync(project => project.Id == id, cancellationToken);
        }

        public async Task<(IReadOnlyList<Project> Items, int TotalCount)> GetPagedAsync(ProjectStatus? status, int page, int pageSize, CancellationToken cancellationToken)
        {
            IQueryable<Project> query = _dbContext.Projects.AsNoTracking();

            if(status.HasValue)
            {
                query = query.Where(project => project.Status == status.Value);
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query.OrderByDescending(project => project.CreatedAt).ThenBy(project => project.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
