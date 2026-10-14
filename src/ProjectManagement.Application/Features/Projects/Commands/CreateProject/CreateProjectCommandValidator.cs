using FluentValidation;

namespace ProjectManagement.Application.Features.Projects.Commands.CreateProject
{
    public class CreateProjectCommandValidator: AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(command => command.OwnerId).NotEmpty().WithMessage("El identificador del propietario es obligatorio.");

            RuleFor(command => command.Name).NotEmpty().WithMessage("El nombre del proyecto es obligatorio.").MaximumLength(80).WithMessage("El nombre del proyecto no puede superar los 80 caracteres.");
        }
    }
}
