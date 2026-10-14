using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Features.Projects.Commands.CreateProject;

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

        // Endpoint provisional
        [HttpGet("{id:guid}")]
        public IActionResult GetById(Guid id)
        {
            return NotFound();
        }
    }
}
