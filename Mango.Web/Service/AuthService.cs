namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseSerivce;
        public AuthService(IBaseService service)
        {
            _baseSerivce = service;
        }
        public async Task<ResponseDTO> AssignRoleAsync(RegistrationDTO reg)
        {
            return await _baseSerivce.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = reg,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDTO> LoginAsync(LoginDTO login)
        {
            return await _baseSerivce.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = login,
                Url = SD.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDTO> RegisterAsync(RegistrationDTO reg)
        {
            return await _baseSerivce.SendAsync(new RequestDTO()
            {
                ApiType = SD.ApiType.POST,
                Data = reg,
                Url = SD.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
