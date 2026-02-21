using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CleanTeethApplication.Contracts.Auth;
using CleanTeethApplication.Features.Auth.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanTeethApplication.Features.Auth.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserManager _userManager;
        private readonly IRoleManager _roleManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserManager userManager,
            IRoleManager roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(registerDto.Email) || 
                    string.IsNullOrWhiteSpace(registerDto.UserName) ||
                    string.IsNullOrWhiteSpace(registerDto.Password))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Email, username, and password are required."
                    };
                }

                // Check if user already exists
                var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "User with this email already exists."
                    };
                }

                // Create new user using the interface
                var createUserRequest = new CreateUserRequest
                {
                    Email = registerDto.Email,
                    UserName = registerDto.UserName,
                    Password = registerDto.Password,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName
                };
                var result = await _userManager.CreateUserAsync(createUserRequest);
                
                if (!result.Success)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = result.Message
                    };
                }

                // Assign role
                var role = registerDto.Role ?? "Dentist";
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = $"Role '{role}' does not exist."
                    };
                }

                var user = await _userManager.FindByEmailAsync(registerDto.Email);
                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Failed to create user."
                    };
                }
                await _userManager.AddToRoleAsync(user, role);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "User registered successfully. Please log in.",
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email ?? string.Empty,
                        UserName = user.UserName ?? string.Empty,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        Roles = new List<string> { role }
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"An error occurred during registration: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Find user by email
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                // Check password
                var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!passwordValid)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid email or password."
                    };
                }

                // Get user roles
                var roles = await _userManager.GetRolesAsync(user);
                var rolesList = new List<string>(roles);

                // Generate tokens
                var accessToken = GenerateAccessToken(user, rolesList);
                var refreshToken = GenerateRefreshToken();

                // Save refresh token to user
                await _userManager.UpdateRefreshTokenAsync(user, refreshToken);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Login successful.",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email ?? string.Empty,
                        UserName = user.UserName ?? string.Empty,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        Roles = rolesList
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"An error occurred during login: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var principal = GetPrincipalFromExpiredToken(refreshTokenDto.AccessToken);
                if (principal == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid token."
                    };
                }

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid token."
                    };
                }

                var user = await _userManager.FindByIdAsync(userIdClaim.Value);
                if (user == null || !await _userManager.ValidateRefreshTokenAsync(user, refreshTokenDto.RefreshToken))
                {
                    return new AuthResponseDto
                    {
                        Success = false,
                        Message = "Invalid or expired refresh token."
                    };
                }

                var roles = await _userManager.GetRolesAsync(user);
                var rolesList = new List<string>(roles);
                var newAccessToken = GenerateAccessToken(user, rolesList);
                var newRefreshToken = GenerateRefreshToken();

                await _userManager.UpdateRefreshTokenAsync(user, newRefreshToken);

                return new AuthResponseDto
                {
                    Success = true,
                    Message = "Token refreshed successfully.",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                    User = new UserInfoDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        FirstName = user.FirstName ?? string.Empty,
                        LastName = user.LastName ?? string.Empty,
                        Roles = rolesList
                    }
                };
            }
            catch (Exception ex)
            {
                return new AuthResponseDto
                {
                    Success = false,
                    Message = $"An error occurred during token refresh: {ex.Message}"
                };
            }
        }

        private string GenerateAccessToken(dynamic user, List<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ??
                "your_super_secret_key_change_this_in_production_at_least_32_characters_long!!!");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    int.Parse(_configuration["JWT:ExpiryInMinutes"] ?? "15")),
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"] ??
                    "your_super_secret_key_change_this_in_production_at_least_32_characters_long!!!");

                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["JWT:Audience"],
                    ValidateLifetime = false // Don't validate lifetime for refresh token
                }, out SecurityToken securityToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }

    // Abstraction interfaces for user and role management
    public interface IUserManager
    {
        Task<dynamic?> FindByEmailAsync(string email);
        Task<dynamic?> FindByIdAsync(string id);
        Task<bool> CheckPasswordAsync(dynamic user, string password);
        Task<IList<string>> GetRolesAsync(dynamic user);
        Task<(bool Success, string Message)> CreateUserAsync(CreateUserRequest userData);
        Task<bool> AddToRoleAsync(dynamic user, string role);
        Task UpdateRefreshTokenAsync(dynamic user, string refreshToken);
        Task<bool> ValidateRefreshTokenAsync(dynamic user, string refreshToken);
    }

    public interface IRoleManager
    {
        Task<bool> RoleExistsAsync(string roleName);
    }
}
