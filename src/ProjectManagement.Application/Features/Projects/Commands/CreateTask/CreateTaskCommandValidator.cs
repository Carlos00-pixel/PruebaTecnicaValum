using FluentValidation;

namespace ProjectManagement.Application.Features.Projects.Commands.CreateTask
{
    public class CreateTaskCommandValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskCommandValidator()
        {
            RuleFor(command => command.ProjectId).NotEmpty().WithMessage("El identificador del proyecto es obligatorio.");

            RuleFor(command => command.Title).NotEmpty().WithMessage("El titulo de la tarea es obligatorio.").MaximumLength(100).WithMessage("El titulo de la tarea no puede superar los 100 caracteres.");

            RuleFor(command => command.Priority).IsInEnum().WithMessage("La prioridad indicada no es válida.");
        }
    }
}
