using System.Net;
using System.Text.Json;
using FluentValidation;
using UniSystem.Domain.Exceptions;

namespace UniSystem.Web.Middleware;

public class ExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var errors = ex.Errors.Select(e => e.ErrorMessage);
            var response = new { error = "Validation failed.", details = errors };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (DomainException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            var response = new { error = ex.Message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            var logger = context.RequestServices.GetRequiredService<ILogger<ExceptionHandlingMiddleware>>();
            logger.LogError(ex, "Unhandled exception occurred.");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var response = env.IsDevelopment()
                ? (object)new { error = "Internal server error.", type = ex.GetType().Name, message = ex.Message }
                : new { error = "Internal server error." };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
