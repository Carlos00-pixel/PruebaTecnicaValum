using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Features.Projects.Commands.CreateTask;
using ProjectManagement.Application.Features.Projects.Commands.UpdateTaskStatus;
using ProjectManagement.Application.Features.Projects.Queries.GetProjectTasks;
using ProjectManagement.Application.Features.Projects.Queries.GetTaskById;
using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class TasksController : ControllerBase
    {
        private readonly ISender _sender;

        public TasksController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("projects/{projectId:guid}/tasks")]
        public async Task<IActionResult> Create(Guid projectId, [FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
        {
            var command = new CreateTaskCommand(projectId, request.Title, request.Description, request.Priority, request.DueDate);

            var taskId = await _sender.Send(command, cancellationToken);

            return Created($"/api/tasks/{taskId}", new { id = taskId });
        }

        [HttpGet("projects/{projectId:guid}/tasks")]
        public async Task<IActionResult> GetByProject(
            Guid projectId, 
            [FromQuery] int page = 1,
            [FromQuery] int  pageSize = 10,
            [FromQuery] TaskItemStatus? status = null,
            [FromQuery] TaskPriority? priority = null,
            CancellationToken cancellationToken = default)
        {
            var query = new GetProjectTasksQuery(projectId, page, pageSize, status, priority);

            var result = await _sender.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("tasks/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetTaskByIdQuery(id);

            var result = await _sender.Send(query, cancellationToken); 
            
            return Ok(result);
        }

        [HttpPatch("tasks/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateTaskStatusRequest request, CancellationToken cancellationToken)
        {
            var command = new UpdateTaskStatusCommand(id, request.Status);

            await _sender.Send(command, cancellationToken); 
            
            return NoContent();
        }

        public record CreateTaskRequest(string Title, string? Description, ProjectManagement.Domain.Enums.TaskPriority Priority, DateTime? DueDate);

        public record UpdateTaskStatusRequest(TaskItemStatus Status);
    }
}
