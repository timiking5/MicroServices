namespace Mango.Web.Models
{
    /// <summary>
    /// Login Request DTO
    /// </summary>
    public class LoginDTO
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
