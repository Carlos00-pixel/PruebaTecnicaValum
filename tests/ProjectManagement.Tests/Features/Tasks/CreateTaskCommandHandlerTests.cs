using Moq;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Features.Projects.Commands.CreateTask;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Tests.Features.Tasks
{
    public class CreateTaskCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ArchivedProject_ThrowsDomainException()
        {
            var projectId = Guid.NewGuid();

            var project = new Project
            {
                Id = projectId,
                Name = "Proyecto archivado",
                Status = ProjectStatus.Archived,
                OwnerId = Guid.NewGuid()
            };

            var projectRepository = new Mock<IProjectRepository>();

            projectRepository.Setup(repository => repository.GetByIdAsync(projectId, It.IsAny<CancellationToken>())).ReturnsAsync(project);

            var taskRepository = new Mock<ITaskRepository>();

            var handler = new CreateTaskCommandHandler(projectRepository.Object, taskRepository.Object);

            var command = new CreateTaskCommand(projectId, "Nueva tarea", null, TaskPriority.High, null);

            await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, CancellationToken.None));

            taskRepository.Verify(repo => repo.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
