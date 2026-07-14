using System.Text.Json;
using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;

namespace OrderProcessingWithDi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;
    private readonly ILogger<ErrorHandlingMiddleware> logger;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (OrderNotFoundException ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
        catch (InvalidOrderException ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
        catch (ArgumentException ex)
        {
            await this.HandleExceptionAsync(context, ex);
        }
        catch (Exception ex) when (
            ex is not OrderNotFoundException &&
            ex is not InvalidOrderException &&
            ex is not ArgumentException)
        {
            await this.HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        Exception exception)
    {
        var errorResponse =
            this.CreateErrorResponse(context, exception);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = errorResponse.Status;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var json =
            JsonSerializer.Serialize(errorResponse, options);

        this.logger.LogError(exception, exception.Message);

        await context.Response.WriteAsync(json);
    }

    private ErrorResponse CreateErrorResponse(
        HttpContext context,
        Exception exception)
    {
        return exception switch
        {
            OrderNotFoundException ex => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "Order Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message,
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object>
                {
                    ["orderId"] = ex.OrderId,
                },
            },

            InvalidOrderException ex => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Invalid Order",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object>(),
            },

            ArgumentException ex => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Invalid Argument",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message,
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object>(),
            },

            _ => new ErrorResponse
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail =
                    "An unexpected error occurred while processing your request.",
                Instance = context.Request.Path,
                Extensions = new Dictionary<string, object>(),
            },
        };
    }
}

public static class ErrorHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseErrorHandling(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlingMiddleware>();
    }
}
