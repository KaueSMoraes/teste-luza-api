using LuzaBlog.Api.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace LuzaBlog.Api.Presentation.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status404NotFound, "Not Found", ex.Message);
        }
        catch (ForbiddenException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status403Forbidden, "Forbidden", ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized", ex.Message);
        }
        catch (ArgumentException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status400BadRequest, "Bad Request", ex.Message);
        }
        catch (Exception ex)
        {
            await WriteProblemAsync(context, StatusCodes.Status500InternalServerError, "Internal Server Error", ex.Message);
        }
    }

    private static async Task WriteProblemAsync(HttpContext context, int status, string title, string detail)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
