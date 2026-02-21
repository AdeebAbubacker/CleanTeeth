using CleanTeeth.Domain.Entities.Exceptions;
using CleanTeethApplication.Common.Response;
using CleanTeethApplication.Exceptions;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CleanTeeth.API.Middlewares
{
    /// <summary>
    /// Global exception handling middleware that catches all unhandled exceptions
    /// and returns standardized error responses
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred. Path: {Path}, Method: {Method}", 
                    context.Request.Path, context.Request.Method);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            ApiResponse response;
            
            switch (exception)
            {
                case NotFoundException notFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = ApiResponse.Failure(notFoundException.Message, 404);
                    break;

                case BusinessRuleException businessRuleException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse.Failure(businessRuleException.Message, 400);
                    break;

                case CustomValidationException validationException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    var errors = new List<string> { validationException.Message };
                    response = ApiResponse.Failure(validationException.Message, 400, errors);
                    break;

                case UnauthorizedAccessException unauthorizedException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response = ApiResponse.Failure(unauthorizedException.Message, 401);
                    break;

                case InvalidOperationException invalidOpException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse.Failure(invalidOpException.Message, 400);
                    break;

                case ArgumentException argException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse.Failure($"Invalid argument: {argException.Message}", 400);
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = ApiResponse.Failure(
                        "An unexpected error occurred. Please contact support if the problem persists.",
                        500,
                        new List<string> { exception.Message }
                    );
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            return context.Response.WriteAsJsonAsync(response, options);
        }
    }

    /// <summary>
    /// Extension method for registering the error handling middleware
    /// </summary>
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
