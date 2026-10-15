using FluentValidation;
using ProjectManagement.Application.Exceptions;
using System.Text.Json;

namespace ProjectManagement.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // Intercepta las excepciones y las transforma en respuestas HTTP con un formato uniforme.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException exception)
            {
                var deatils = exception.Errors.Select(error => error.ErrorMessage).Distinct().ToArray();

                await WriteErrorAsync(context, StatusCodes.Status400BadRequest, nameof(ValidationException), "La petición contiene datos incorrectos", deatils);
            }
            catch (NotFoundException exception)
            {
                await WriteErrorAsync(context, StatusCodes.Status404NotFound, nameof(NotFoundException), exception.Message);
            }
            catch (DomainException exception)
            {
                await WriteErrorAsync(context, StatusCodes.Status422UnprocessableEntity, nameof(DomainException), exception.Message);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Se ha producido un erorr inesperado.");

                await WriteErrorAsync(context, StatusCodes.Status500InternalServerError, "InternalServerError", "Se ha producido un error interno.");
            }
        }

        // Devuelve los errores con una estructura JSON uniforme para toda la API.
        private static async Task WriteErrorAsync(HttpContext context, int statusCode, string error, string message, string[]? details = null)
        {
            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                status = statusCode,
                error,
                message,
                details = details ?? Array.Empty<string>()
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
