using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginDTO model = new();
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>() {
                new SelectListItem{ Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{ Text = SD.RoleCustomer, Value = SD.RoleCustomer}
            };
            ViewBag.RoleList = roleList;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationDTO request)
        {
            ResponseDTO response = await _authService.RegisterAsync(request);
            ResponseDTO? assignRole = new();
            if (response is not null && response.IsSuccess)
            {
                request.Role = string.IsNullOrEmpty(request.Role) ? SD.RoleCustomer : request.Role;
                assignRole = await _authService.AssignRoleAsync(request);
            }
            if (assignRole is not null && assignRole.IsSuccess)
            {
                TempData["success"] = "Registration Successful";
                return RedirectToAction("Login");
            }
            var roleList = new List<SelectListItem>() {
                new SelectListItem{ Text = SD.RoleAdmin, Value = SD.RoleAdmin},
                new SelectListItem{ Text = SD.RoleCustomer, Value = SD.RoleCustomer}
            };
            TempData["error"] = response.Message;
            ViewBag.RoleList = roleList;
            return View(request);
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO request)
        {
            ResponseDTO response = await _authService.LoginAsync(request);
            if (response is not null && response.IsSuccess)
            {
                LoginResponseDTO loginResp = JsonConvert.DeserializeObject<LoginResponseDTO>(
                    Convert.ToString(response.Result));
                _tokenProvider.SetToken(loginResp.Token);
                await SignInUser(loginResp);
                TempData["success"] = "Logged in Successfully";
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("CustomError", response.Message);
            return View(request);
        }
        private async Task SignInUser(LoginResponseDTO model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(
                x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(
                x => x.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(
                x => x.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(
                x => x.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(
                x => x.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
