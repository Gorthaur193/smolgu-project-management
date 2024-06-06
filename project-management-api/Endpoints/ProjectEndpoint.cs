
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Projects;
using ProjectManagement.Core.DTOs;

namespace project_management_api.Endpoints;

public static class ProjectEndpoint
{

    public static void MapProjectEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/projects", GetProjects);
        routes.MapPost("/projects/", AddProject);
    }

    private static async Task<IResult> AddProject([FromBody]ProjectDTO projectDTO, ProjectService service, CancellationToken cancellationToken)
    {
        try
        {
            await service.CreateOrUpdateProjectAsync(projectDTO, cancellationToken);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    private static async Task<IResult> GetProjects(ProjectService service, CancellationToken cancellationToken)
    {
        try
        {
            return Results.Ok(await service.GetProjectsAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }
}
