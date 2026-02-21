using Microsoft.AspNetCore.Identity;
using CleanTeethApplication.Features.Auth.Services;

namespace CleanTeethPersistance.Auth
{
    public class UserManagerAdapter : IUserManager
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerAdapter(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<dynamic?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<dynamic?> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> CheckPasswordAsync(dynamic user, string password)
        {
            if (user == null) return false;
            var appUser = user as ApplicationUser;
            if (appUser == null) return false;
            return await _userManager.CheckPasswordAsync(appUser, password);
        }

        public async Task<IList<string>> GetRolesAsync(dynamic user)
        {
            if (user == null) return new List<string>();
            var appUser = user as ApplicationUser;
            if (appUser == null) return new List<string>();
            return await _userManager.GetRolesAsync(appUser);
        }

        public async Task<(bool Success, string Message)> CreateUserAsync(CreateUserRequest userData)
        {
            try
            {
                if (string.IsNullOrEmpty(userData.Email) || string.IsNullOrEmpty(userData.UserName) || string.IsNullOrEmpty(userData.Password))
                {
                    return (false, "Email, UserName, and Password are required");
                }

                var user = new ApplicationUser
                {
                    Email = userData.Email,
                    UserName = userData.UserName,
                    FirstName = userData.FirstName,
                    LastName = userData.LastName
                };

                var result = await _userManager.CreateAsync(user, userData.Password);
                if (!result.Succeeded)
                {
                    var errorList = new List<string>();
                    foreach (var error in result.Errors)
                    {
                        errorList.Add(error.Description);
                    }
                    var errors = string.Join(", ", errorList);
                    return (false, $"Failed to create user: {errors}");
                }

                return (true, "User created successfully");
            }
            catch (Exception ex)
            {
                return (false, $"Error creating user: {ex.Message}");
            }
        }

        public async Task<bool> AddToRoleAsync(dynamic user, string role)
        {
            if (user == null) return false;
            var appUser = user as ApplicationUser;
            if (appUser == null) return false;
            var result = await _userManager.AddToRoleAsync(appUser, role);
            return result.Succeeded;
        }

        public async Task UpdateRefreshTokenAsync(dynamic user, string refreshToken)
        {
            if (user != null)
            {
                var appUser = user as ApplicationUser;
                if (appUser != null)
                {
                    appUser.RefreshToken = refreshToken;
                    appUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                    await _userManager.UpdateAsync(appUser);
                }
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync(dynamic user, string refreshToken)
        {
            return await Task.FromResult(user == null ? false : (user as ApplicationUser).RefreshToken == refreshToken && (user as ApplicationUser).RefreshTokenExpiryTime > DateTime.UtcNow);
        }
    }

    public class RoleManagerAdapter : IRoleManager
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagerAdapter(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }
    }
}

