using ProjectManagement.Application.Features.Projects.Commands.CreateTask;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Tests.Features.Tasks;

public class CreateTaskCommandValidatorTests
{
    private readonly CreateTaskCommandValidator _validator = new();

    [Fact]
    public void Validate_ValidCommand_Passes()
    {
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            "Implementar pruebas",
            "Descripción de prueba",
            TaskPriority.High,
            null);

        var result = _validator.Validate(command);

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_EmptyTitle_Fails(string title)
    {
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            title,
            null,
            TaskPriority.High,
            null);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(command.Title));
    }

    [Fact]
    public void Validate_TitleTooLong_Fails()
    {
        var command = new CreateTaskCommand(
            Guid.NewGuid(),
            new string('A', 101),
            null,
            TaskPriority.High,
            null);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(command.Title));
    }

    [Fact]
    public void Validate_EmptyProjectId_Fails()
    {
        var command = new CreateTaskCommand(
            Guid.Empty,
            "Tarea válida",
            null,
            TaskPriority.High,
            null);

        var result = _validator.Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == nameof(command.ProjectId));
    }
}
