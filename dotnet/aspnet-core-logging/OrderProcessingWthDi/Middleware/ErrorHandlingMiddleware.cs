using Microsoft.AspNetCore.Mvc;
using OrderProcessingWithDi.Models.Exceptions;

namespace OrderProcessingWithDi.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (OrderNotFoundException exception)
        {
            await this.HandleOrderNotFoundException(context, exception);
        }
        catch (InvalidOrderException exception)
        {
            await this.HandleInvalidOrderException(context, exception);
        }
        catch (ArgumentException exception)
        {
            await this.HandleArgumentException(context, exception);
        }
    }

    private async Task HandleOrderNotFoundException(
        HttpContext context,
        OrderNotFoundException exception)
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        context.Response.ContentType = "application/json";

        var orderId = this.ExtractOrderId(exception.Message);

        var response = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Title = "Order Not Found",
            Status = 404,
            Detail = exception.Message,
            Instance = context.Request.Path.ToString()
        };

        response.Extensions["orderId"] = orderId;

        await context.Response.WriteAsJsonAsync(response);
    }

    private async Task HandleInvalidOrderException(
        HttpContext context,
        InvalidOrderException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var response = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Invalid Order",
            Status = 400,
            Detail = exception.Message,
            Instance = context.Request.Path.ToString()
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private async Task HandleArgumentException(
        HttpContext context,
        ArgumentException exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var response = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Bad Request",
            Status = 400,
            Detail = exception.Message,
            Instance = context.Request.Path.ToString()
        };

        await context.Response.WriteAsJsonAsync(response);
    }

    private int ExtractOrderId(string message)
    {
        var parts = message.Split(' ');

        foreach (var part in parts)
        {
            if (int.TryParse(part, out var orderId))
            {
                return orderId;
            }
        }

        return 0;
    }
}
