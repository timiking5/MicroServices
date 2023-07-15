namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDTO> LoginAsync(LoginDTO login);
        Task<ResponseDTO> RegisterAsync(RegistrationDTO reg);
        Task<ResponseDTO> AssignRoleAsync(RegistrationDTO reg);
    }
}
