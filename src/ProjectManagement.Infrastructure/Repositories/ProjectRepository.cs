using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Domain.Entities;
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
    }
}
