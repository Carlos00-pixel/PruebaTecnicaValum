using MediatR;
using ProjectManagement.Application.Abstractions;

namespace ProjectManagement.Application.Features.Projects.Commands.DeleteTask
{
    public class DeleteTaskCommandHandler: IRequestHandler<DeleteTaskCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

            // Si la tarea no existe o ya está eliminada, se devuelve igualmente una respuesta satisfactoria.
            if (task is null)
            {
                return Unit.Value;
            }

            // Realiza un borrado lógico para conservar el registro en la base de datos.
            task.IsDeleted = true;

            await _taskRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
