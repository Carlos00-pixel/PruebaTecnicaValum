using MediatR;
using ProjectManagement.Application.Abstractions;
using ProjectManagement.Application.Common;
using ProjectManagement.Application.Exceptions;

namespace ProjectManagement.Application.Features.Projects.Queries.GetProjectTasks;

public class GetProjectTasksQueryHandler: IRequestHandler<GetProjectTasksQuery, PagedResult<TaskListItemDto>>
{
    private readonly ITaskRepository _taskRepository;
    private readonly IProjectRepository _projectRepository;

    public GetProjectTasksQueryHandler(ITaskRepository taskRepository, IProjectRepository projectRepository)
    {
        _taskRepository = taskRepository;
        _projectRepository = projectRepository;
    }

    public async Task<PagedResult<TaskListItemDto>> Handle(GetProjectTasksQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId, cancellationToken);

        if (project is null)
        {
            throw new NotFoundException($"No se ha encontrado el proyecto con el identificador: {request.ProjectId}.");
        }

        var page = Math.Max(request.Page, 1);
        var pageSize = request.PageSize <= 0 ? 10 : Math.Min(request.PageSize, 50);

        var (items, totalCount) = await _taskRepository.GetPagedAsync(request.ProjectId, request.Status, request.Priority, page, pageSize, cancellationToken);

        var taskDtos = items.Select(task => new TaskListItemDto(task.Id, task.ProjectId, task.Title, task.Description, task.Priority, task.Status, task.DueDate, task.CreatedAt, task.CompletedAt)).ToList();

        return new PagedResult<TaskListItemDto>(taskDtos, page, pageSize, totalCount);
    }
}
