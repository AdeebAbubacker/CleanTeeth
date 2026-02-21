using CleanTeethApplication.Common.Response;
using CleanTeethApplication.Contracts.Auth;
using CleanTeethApplication.Features.Auth.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CleanTeeth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, ILogger<AuthController> logger) : base(logger)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user account
        /// </summary>
        /// <param name="registerDto">Registration details</param>
        /// <returns>User info and success message</returns>
        /// <response code="200">User registered successfully</response>
        /// <response code="400">Invalid input or user already exists</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<object>>> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse("Validation failed", GetModelStateErrors());
            }

            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Registration failed: {Message}", result.Message);
                    return BadRequestResponse(result.Message);
                }

                _logger.LogInformation("User registered successfully: {UserName}", registerDto.UserName);
                return SuccessResponse<object>(result.User, result.Message, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during registration");
                return InternalErrorResponse("An error occurred during registration", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// User login with email and password
        /// </summary>
        /// <param name="loginDto">Login credentials</param>
        /// <returns>JWT tokens and user info</returns>
        /// <response code="200">Login successful</response>
        /// <response code="400">Invalid credentials format</response>
        /// <response code="401">Invalid email or password</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse("Validation failed", GetModelStateErrors());
            }

            try
            {
                var result = await _authService.LoginAsync(loginDto);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Login failed for email: {Email}", loginDto.Email);
                    return Unauthorized(ApiResponse.Failure(result.Message, 401));
                }

                _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
                return SuccessResponse<object>(new
                {
                    result.AccessToken,
                    result.RefreshToken,
                    result.User
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return InternalErrorResponse("An error occurred during login", new List<string> { ex.Message });
            }
        }

        /// <summary>
        /// Refresh expired access token
        /// </summary>
        /// <param name="refreshTokenDto">Current access token and refresh token</param>
        /// <returns>New JWT tokens</returns>
        /// <response code="200">Token refreshed successfully</response>
        /// <response code="400">Invalid token format</response>
        /// <response code="401">Invalid or expired refresh token</response>
        /// <response code="500">Internal server error</response>
        [HttpPost("refresh-token")]
        public async Task<ActionResult<ApiResponse<object>>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse("Validation failed", GetModelStateErrors());
            }

            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenDto);
                
                if (!result.Success)
                {
                    _logger.LogWarning("Token refresh failed: {Message}", result.Message);
                    return Unauthorized(ApiResponse.Failure(result.Message, 401));
                }

                _logger.LogInformation("Token refreshed successfully");
                return SuccessResponse<object>(new
                {
                    result.AccessToken,
                    result.RefreshToken,
                    result.User
                }, result.Message, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during token refresh");
                return InternalErrorResponse("An error occurred during token refresh", new List<string> { ex.Message });
            }
        }
    }
}
