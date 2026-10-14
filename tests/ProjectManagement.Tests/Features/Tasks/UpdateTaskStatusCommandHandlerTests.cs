using Moq;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Features.Projects.Commands.UpdateTaskStatus;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Tests.Features.Tasks;

public class UpdateTaskStatusCommandHandlerTests
{
    [Fact]
    public async Task Handle_StatusDone_SetsCompletedAt()
    {
        var taskId = Guid.NewGuid();

        var task = new TaskItem
        {
            Id = taskId,
            ProjectId = Guid.NewGuid(),
            Title = "Tarea de prueba",
            Status = TaskItemStatus.InProgress,
            CompletedAt = null
        };

        var taskRepository = new Mock<ITaskRepository>();

        taskRepository.Setup(repository => repository.GetByIdAsync(taskId, It.IsAny<CancellationToken>())).ReturnsAsync(task);

        var handler = new UpdateTaskStatusCommandHandler(taskRepository.Object);

        var command = new UpdateTaskStatusCommand(taskId, TaskItemStatus.Done);

        await handler.Handle(command, CancellationToken.None);

        Assert.Equal(TaskItemStatus.Done, task.Status);
        Assert.NotNull(task.CompletedAt);
        Assert.Equal(DateTimeKind.Utc, task.CompletedAt!.Value.Kind);

        taskRepository.Verify(repository => repository.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
