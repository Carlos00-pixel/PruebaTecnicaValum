using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Features.Projects.Commands.CreateTask;

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

        public record CreateTaskRequest(string Title, string? Description, ProjectManagement.Domain.Enums.TaskPriority Priority, DateTime? DueDate);
    }
}
