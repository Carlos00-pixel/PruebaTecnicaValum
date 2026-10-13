using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public ProjectStatus Status { get; set; } = ProjectStatus.Actived;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Guid OwnerId { get; set; }

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
