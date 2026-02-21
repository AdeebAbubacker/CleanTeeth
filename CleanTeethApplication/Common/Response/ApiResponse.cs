using System.Net;

namespace CleanTeethApplication.Common.Response
{
    /// <summary>
    /// Standard API response wrapper for typed endpoints
    /// </summary>
    public class ApiResponse<T>
    {
        [System.Text.Json.Serialization.JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("data")]
        public T? Data { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("errors")]
        public List<string>? Errors { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Creates a success response
        /// </summary>
        public static ApiResponse<T> SuccessResponse(T? data, string message = "Request successful", int statusCode = 200)
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = null
            };
        }

        /// <summary>
        /// Creates a failure response
        /// </summary>
        public static ApiResponse<T> FailureResponse(string message, int statusCode = 400, List<string>? errors = null, T? data = default)
        {
            return new ApiResponse<T>
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Errors = errors
            };
        }

        /// <summary>
        /// Creates a created response (201)
        /// </summary>
        public static ApiResponse<T> Created(T? data, string message = "Resource created successfully")
        {
            return SuccessResponse(data, message, 201);
        }

        /// <summary>
        /// Creates a not found response (404)
        /// </summary>
        public static ApiResponse<T> NotFound(string message = "Resource not found")
        {
            return FailureResponse(message, 404);
        }

        /// <summary>
        /// Creates an unauthorized response (401)
        /// </summary>
        public static ApiResponse<T> Unauthorized(string message = "Unauthorized access")
        {
            return FailureResponse(message, 401);
        }

        /// <summary>
        /// Creates an internal server error response (500)
        /// </summary>
        public static ApiResponse<T> InternalServerError(string message = "Internal server error", List<string>? errors = null)
        {
            return FailureResponse(message, 500, errors);
        }

        // Convenience method names for backward compatibility
        public static ApiResponse<T> Success(T? data, string message = "Request successful", int statusCode = 200)
            => SuccessResponse(data, message, statusCode);

        public static ApiResponse<T> Failure(string message, int statusCode = 400, List<string>? errors = null, T? data = default)
            => FailureResponse(message, statusCode, errors, data);
    }

    /// <summary>
    /// Standard API response wrapper for untyped endpoints
    /// </summary>
    public class ApiResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("data")]
        public object? Data { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("errors")]
        public List<string>? Errors { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponse SuccessResponse(object? data, string message = "Request successful", int statusCode = 200)
        {
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = statusCode,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };
        }

        public static ApiResponse FailureResponse(string message, int statusCode = 400, List<string>? errors = null)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = statusCode,
                Message = message,
                Errors = errors,
                Timestamp = DateTime.UtcNow
            };
        }

        // Convenience method names for backward compatibility
        public static ApiResponse Success(object? data, string message = "Request successful", int statusCode = 200)
            => SuccessResponse(data, message, statusCode);

        public static ApiResponse Failure(string message, int statusCode = 400, List<string>? errors = null)
            => FailureResponse(message, statusCode, errors);
    }
}
