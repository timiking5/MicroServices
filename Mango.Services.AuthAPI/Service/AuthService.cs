using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.DTO;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == email.ToLower());
            if (user is null)
            {
                return false;
            }
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
            await _userManager.AddToRoleAsync(user, roleName);
            return true;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO login)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.Email.ToLower() == login.Email.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (user is null || !isValid)
            {
                return new LoginResponseDTO()
                {
                    User = null,
                    Token = ""
                };
            }
            var roles = await _userManager.GetRolesAsync(user);
            UserDTO userDto = new()
            {
                Email = user.Email,
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDTO response = new()
            {
                User = userDto,
                Token = _jwtTokenGenerator.GenerateToken(user, roles)
            };
            return response;
        }

        public async Task<string> Register(RegistrationDTO reg)
        {
            ApplicationUser user = new()
            {
                UserName = reg.Email,
                Email = reg.Email,
                NormalizedEmail = reg.Email.ToUpper(),
                FirstName = reg.FirstName,
                LastName = reg.LastName,
                PhoneNumber = reg.PhoneNumber
            };
            try
            {
                var result = await _userManager.CreateAsync(user, reg.Password);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(x => x.UserName == user.UserName);
                    UserDTO userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        FirstName = userToReturn.FirstName,
                        LastName = userToReturn.LastName,
                        PhoneNumber = userToReturn.PhoneNumber
                    };
                    return "";
                }
                return result.Errors.FirstOrDefault().Description;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}
