using OrderProcessingWithDi.Models;
using OrderProcessingWithDi.Models.Exceptions;

namespace OrderProcessingWithDi.Middleware;

/// <summary>
/// Middleware for global error handling.
/// Catches exceptions and converts them to standardized error responses.
/// TODO: Implement error handling middleware.
/// 
/// Requirements:
/// 1. Add constructor with RequestDelegate next and ILogger&lt;ErrorHandlingMiddleware&gt; logger
/// 2. Implement InvokeAsync method:
///    - Wrap await next(context) in try-catch
///    - Catch OrderNotFoundException, InvalidOrderException, ArgumentException, and Exception
///    - Call HandleExceptionAsync for each caught exception
/// 3. Implement HandleExceptionAsync method:
///    - Create ErrorResponse using CreateErrorResponse
///    - Set ContentType to "application/json"
///    - Set StatusCode to errorResponse.Status
///    - Serialize errorResponse to JSON with camelCase naming
///    - Write JSON to response
///    - Log error using logger.LogError
/// 4. Implement CreateErrorResponse method using switch expression:
///    - OrderNotFoundException → Status 404, Type "rfc7231#section-6.5.4", Title "Order Not Found"
///      Add orderId to Extensions
///    - InvalidOrderException → Status 400, Type "rfc7231#section-6.5.1", Title "Invalid Order"
///    - ArgumentException → Status 400, Type "rfc7231#section-6.5.1", Title "Invalid Argument"
///    - Default → Status 500, Type "rfc7231#section-6.6.1", Title "Internal Server Error"
///    - Set Instance to context.Request.Path for all
///    - Set Detail to exception.Message (or generic message for default case)
/// </summary>
public class ErrorHandlingMiddleware
{
    // TODO: Add fields for RequestDelegate and ILogger
    
    // TODO: Implement constructor
    
    // TODO: Implement InvokeAsync method
    
    // TODO: Implement HandleExceptionAsync method
    
    // TODO: Implement CreateErrorResponse method
}

/// <summary>
/// Extension methods for registering error handling middleware.
/// TODO: Implement extension method.
/// 
/// Requirements:
/// - Create static class ErrorHandlingMiddlewareExtensions
/// - Add static method UseErrorHandling(this IApplicationBuilder builder)
///   - Should call builder.UseMiddleware&lt;ErrorHandlingMiddleware&gt;()
///   - Should return builder for chaining
/// </summary>
public static class ErrorHandlingMiddlewareExtensions
{
    // TODO: Implement UseErrorHandling extension method
}



