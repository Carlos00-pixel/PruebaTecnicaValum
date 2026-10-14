using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Exceptions;
using ProjectManagement.Application.Features.Projects.Queries.GetProjectById;
using ProjectManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Features.Projects.Queries.GetTaskById
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailsDto>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDetailsDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.Id, cancellationToken);

            if (task is null)
            {
                throw new NotFoundException($"No se ha encontrado la tarea con el identificador: {request.Id}.");
            }

            return new TaskDetailsDto(
                task.Id,
                task.ProjectId,
                task.Title,
                task.Description,
                task.Priority,
                task.Status,
                task.DueDate,
                task.CreatedAt,
                task.CompletedAt);
        }
    }
}
