using FluentValidation;

namespace ProjectManagement.Application.Features.Projects.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandValidator: AbstractValidator<UpdateTaskStatusCommand>
    {
        public UpdateTaskStatusCommandValidator()
        {
            RuleFor(command => command.TaskId).NotEmpty().WithMessage("El identificador de la tarea es obligatorio.");

            RuleFor(command => command.Status).IsInEnum().WithMessage("El estado indicado no es válido.");
        }
    }
}
