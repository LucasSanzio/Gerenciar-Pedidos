using System.Net;
using System.Text.Json;

namespace Reporting.Api.Middlewares;

/// <summary>
/// Middleware responsável por padronizar respostas de erro em JSON.
/// </summary>
public class JsonExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<JsonExceptionHandlingMiddleware> _logger;

    public JsonExceptionHandlingMiddleware(RequestDelegate next, ILogger<JsonExceptionHandlingMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado processando requisição.");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Message = "Ocorreu um erro inesperado."
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}

public static class JsonExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseJsonExceptionHandling(this IApplicationBuilder app)
    {
        return app.UseMiddleware<JsonExceptionHandlingMiddleware>();
    }
}
