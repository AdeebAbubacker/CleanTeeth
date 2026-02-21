using CleanTeethApplication.Common.Response;
using CleanTeethApplication.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    /// <summary>
    /// Base controller providing common functionality for all API controllers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public abstract class BaseController : ControllerBase
    {
        protected readonly ILogger<BaseController> _logger;

        protected BaseController(ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns a success response with data
        /// </summary>
        protected ActionResult<ApiResponse<T>> SuccessResponse<T>(T? data, string message = "Request successful", int statusCode = 200)
        {
            return StatusCode(statusCode, ApiResponse<T>.Success(data, message, statusCode));
        }

        /// <summary>
        /// Returns a created response (201)
        /// </summary>
        protected ActionResult<ApiResponse<T>> CreatedResponse<T>(T? data, string message = "Resource created successfully")
        {
            return StatusCode(201, ApiResponse<T>.Created(data, message));
        }

        /// <summary>
        /// Returns a no content response (204)
        /// </summary>
        protected ActionResult NoContentResponse(string message = "No content")
        {
            return NoContent();
        }

        /// <summary>
        /// Returns a bad request response (400)
        /// </summary>
        protected ActionResult BadRequestResponse(string message, List<string>? errors = null)
        {
            return BadRequest(ApiResponse.Failure(message, 400, errors));
        }

        /// <summary>
        /// Returns a not found response (404)
        /// </summary>
        protected ActionResult NotFoundResponse(string message = "Resource not found")
        {
            return NotFound(ApiResponse.Failure(message, 404));
        }

        /// <summary>
        /// Returns an unauthorized response (401)
        /// </summary>
        protected ActionResult UnauthorizedResponse(string message = "Unauthorized access")
        {
            return Unauthorized(ApiResponse.Failure(message, 401));
        }

        /// <summary>
        /// Returns a conflict response (409)
        /// </summary>
        protected ActionResult ConflictResponse(string message = "Resource conflict")
        {
            return Conflict(ApiResponse.Failure(message, 409));
        }

        /// <summary>
        /// Returns an internal server error response (500)
        /// </summary>
        protected ActionResult InternalErrorResponse(string message = "Internal server error", List<string>? errors = null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse.Failure(message, 500, errors));
        }

        /// <summary>
        /// Extract validation errors from ModelState
        /// </summary>
        protected List<string> GetModelStateErrors()
        {
            return ModelState.Values
                .SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                .ToList();
        }

        /// <summary>
        /// Helper for handling CQRS Queries
        /// </summary>
        protected async Task<ActionResult<ApiResponse<TResponse>>> HandleQueryAsync<TResponse>(
            IMediator mediator,
            IRequest<TResponse> query,
            string successMessage = "Request successful",
            int successStatusCode = 200)
        {
            try
            {
                var result = await mediator.Send(query);

                if (result == null || (result is System.Collections.IEnumerable enumerable && !enumerable.GetEnumerator().MoveNext()))
                {
                    _logger.LogInformation("No records found for query {QueryName}", query.GetType().Name);
                    return SuccessResponse<TResponse>(result, "No records found", 200);
                }

                _logger.LogInformation("Successfully executed query {QueryName}", query.GetType().Name);
                return SuccessResponse<TResponse>(result, successMessage, successStatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing query {QueryName}", query.GetType().Name);
                return InternalErrorResponse("An error occurred while processing the request", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Helper for handling CQRS Commands that don't return data
        /// </summary>
        protected async Task<ActionResult> HandleCommandAsync(
            IMediator mediator,
            IRequest command,
            string successMessage = "Operation successful")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequestResponse("Validation failed", GetModelStateErrors());
                }

                await mediator.Send(command);

                _logger.LogInformation("Successfully executed command {CommandName}", command.GetType().Name);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing command {CommandName}", command.GetType().Name);
                return InternalErrorResponse("An error occurred while processing the request", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Helper for handling CQRS Commands that return data (e.g. Create returning ID)
        /// </summary>
        protected async Task<ActionResult<ApiResponse<object>>> HandleCreatedCommandAsync<TResponse>(
            IMediator mediator,
            IRequest<TResponse> command,
            string successMessage = "Resource created successfully")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequestResponse("Validation failed", GetModelStateErrors());
                }

                var result = await mediator.Send(command);

                _logger.LogInformation("Successfully executed command {CommandName}", command.GetType().Name);
                return CreatedResponse<object>(result, successMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing command {CommandName}", command.GetType().Name);
                return InternalErrorResponse("An error occurred while processing the request", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Helper for handling CQRS Commands that return data (e.g. Complete appointment returning ID)
        /// </summary>
        protected async Task<ActionResult<ApiResponse<object>>> HandleCommandWithResultAsync<TResponse>(
            IMediator mediator,
            IRequest<TResponse> command,
            string successMessage = "Operation successful")
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequestResponse("Validation failed", GetModelStateErrors());
                }

                var result = await mediator.Send(command);

                _logger.LogInformation("Successfully executed command {CommandName}", command.GetType().Name);
                return SuccessResponse<object>(result, successMessage, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing command {CommandName}", command.GetType().Name);
                return InternalErrorResponse("An error occurred while processing the request", new List<string> { ex.Message });
            }
        }
    }
}
