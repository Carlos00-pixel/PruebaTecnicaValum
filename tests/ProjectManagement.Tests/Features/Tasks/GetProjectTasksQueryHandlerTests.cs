using Moq;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Features.Projects.Queries.GetProjectTasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Tests.Features.Tasks;

public class GetProjectTasksQueryHandlerTests
{
    [Fact]
    public async Task Handle_ValidQuery_ReturnsMappedPagedResult()
    {
        var projectId = Guid.NewGuid();

        var project = new Project
        {
            Id = projectId,
            Name = "Proyecto de prueba",
            OwnerId = Guid.NewGuid(),
            Status = ProjectStatus.Actived
        };

        var tasks = new List<TaskItem>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                Title = "Primera tarea",
                Description = "Descripción de prueba",
                Priority = TaskPriority.High,
                Status = TaskItemStatus.Done,
                CreatedAt = DateTime.UtcNow,
                CompletedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProjectId = projectId,
                Title = "Segunda tarea",
                Priority = TaskPriority.Low,
                Status = TaskItemStatus.Todo,
                CreatedAt = DateTime.UtcNow
            }
        };

        var projectRepository = new Mock<IProjectRepository>();

        projectRepository.Setup(repository => repository.GetByIdAsync(projectId, It.IsAny<CancellationToken>())).ReturnsAsync(project);

        var taskRepository = new Mock<ITaskRepository>();

        taskRepository
            .Setup(repository => repository.GetPagedAsync(
                projectId,
                null,
                null,
                2,
                2,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(((IReadOnlyList<TaskItem>)tasks, 5));

        var handler = new GetProjectTasksQueryHandler(taskRepository.Object, projectRepository.Object);

        var query = new GetProjectTasksQuery(projectId, Page: 2, PageSize: 2);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.Equal(2, result.Page);
        Assert.Equal(2, result.PageSize);
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Items.Count);

        var firstTask = result.Items[0];

        Assert.Equal(tasks[0].Id, firstTask.Id);
        Assert.Equal(projectId, firstTask.ProjectId);
        Assert.Equal("Primera tarea", firstTask.Title);
        Assert.Equal("Descripción de prueba", firstTask.Description);
        Assert.Equal(TaskPriority.High, firstTask.Priority);
        Assert.Equal(TaskItemStatus.Done, firstTask.Status);
        Assert.NotNull(firstTask.CompletedAt);

        Assert.Equal("Segunda tarea", result.Items[1].Title);

        taskRepository.Verify(
            repository => repository.GetPagedAsync(
                projectId,
                null,
                null,
                2,
                2,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
