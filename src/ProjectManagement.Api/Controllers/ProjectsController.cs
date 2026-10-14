using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Features.Projects.Commands.CreateProject;
using ProjectManagement.Application.Features.Projects.Queries.GetProjectById;
using ProjectManagement.Application.Features.Projects.Queries.GetProjects;
using ProjectManagement.Domain.Enums;

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

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] ProjectStatus? status = null, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetProjectsQuery(page, pageSize, status), cancellationToken);

            return Ok(result);
        }
    }
}
