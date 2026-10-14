using ProjectManagement.Application.Features.Projects.Commands.UpdateTaskStatus;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Tests.Features.Tasks;

public class UpdateTaskStatusCommandValidatorTests
{
    private readonly UpdateTaskStatusCommandValidator _validator = new();

    [Theory]
    [InlineData(TaskItemStatus.Todo)]
    [InlineData(TaskItemStatus.InProgress)]
    [InlineData(TaskItemStatus.Done)]
    public void Validate_ValidStatus_Passes(TaskItemStatus status)
    {
        var command = new UpdateTaskStatusCommand(Guid.NewGuid(), status);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Fact]
    public void Validate_InvalidStatus_Fails()
    {
        var command = new UpdateTaskStatusCommand(Guid.NewGuid(), (TaskItemStatus)99);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(command.Status));
    }

    [Fact]
    public void Validate_EmptyTaskId_Fails()
    {
        var command = new UpdateTaskStatusCommand(Guid.Empty, TaskItemStatus.Done);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(command.TaskId));
    }
}
