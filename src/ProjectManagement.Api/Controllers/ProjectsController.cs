using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Features.Projects.Commands.CreateProject;
using ProjectManagement.Application.Features.Projects.Queries.GetProjectById;

namespace ProjectManagement.Api.Controllers
{

    [ApiController]
    [Route("api/projects")]
    public class ProjectsController: ControllerBase
    {
        private readonly ISender _sender;

        public ProjectsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken cancellationToken)
        {
            var projectId = await _sender.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetById), new { id = projectId }, new { id = projectId});
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            var project = await _sender.Send(new GetProjectByIdQuery(id), cancellationToken);

            return Ok(project);
        }
    }
}
