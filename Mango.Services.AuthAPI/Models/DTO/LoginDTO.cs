namespace Mango.Services.AuthAPI.Models.DTO
{
    /// <summary>
    /// Login Request DTO
    /// </summary>
    public class LoginDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
