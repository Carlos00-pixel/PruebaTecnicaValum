using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.Features.Projects.Commands.UpdateTaskStatus
{
    public class UpdateTaskStatusCommandHandler: IRequestHandler<UpdateTaskStatusCommand, Unit>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskStatusCommandHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Unit> Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId, cancellationToken);

            if(task is null)
            {
                throw new NotFoundException($"No se ha encontrado la tarea con el identificador: {request.TaskId}.");
            }

            task.Status = request.Status;

            // Registra la fecha de finalización únicamente cuando la tarea está completada.
            if (request.Status == TaskItemStatus.Done)
            {
                task.CompletedAt = DateTime.UtcNow;
            }
            else
            {
                task.CompletedAt = null;
            }

            await _taskRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
