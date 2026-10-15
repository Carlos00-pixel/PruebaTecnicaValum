using Microsoft.EntityFrameworkCore;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Project> Projects => Set<Project>();

        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configura las restricciones y la relación entre proyectos y tareas.
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(80);

                entity.Property(p => p.Description);

                entity.Property(p => p.Status).HasConversion<string>().IsRequired();

                entity.Property(p => p.CreatedAt).IsRequired();

                entity.Property(p => p.OwnerId).IsRequired();

                entity.HasMany(p => p.Tasks).WithOne(t => t.Project).HasForeignKey(t => t.ProjectId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });

            // Define las restricciones de los campos y los valores predeterminados de las tareas.
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Title).IsRequired().HasMaxLength(100);

                entity.Property(t => t.Description);

                entity.Property(t => t.Priority).HasConversion<string>().IsRequired();

                entity.Property(t => t.Status).HasConversion<string>().IsRequired();

                entity.Property(t => t.DueDate);

                entity.Property(t => t.CreatedAt).IsRequired();

                entity.Property(t => t.CompletedAt);

                entity.Property(t => t.IsDeleted).IsRequired().HasDefaultValue(false);
            });

            // Excluye automáticamente las tareas eliminadas de las consultas habituales.
            modelBuilder.Entity<TaskItem>().HasQueryFilter(t => !t.IsDeleted);
        }
    }
}
