using System.Net;
using System.Text.Json;

namespace TodoApi.Exceptions
{
    /// <summary>
    /// Centralised error handling. Prevents raw exception messages/stack traces from
    /// leaking to clients (a security concern in the original code, where every
    /// controller action returned ex.Message in the response body).
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (TodoNotFoundException ex)
            {
                _logger.LogWarning(ex, "Todo not found");
                await WriteProblemAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception processing {Method} {Path}", context.Request.Method, context.Request.Path);
                await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
            }
        }

        private static async Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string detail)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var problem = new
            {
                status = (int)statusCode,
                title = statusCode == HttpStatusCode.NotFound ? "Not Found" : "Internal Server Error",
                detail
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
