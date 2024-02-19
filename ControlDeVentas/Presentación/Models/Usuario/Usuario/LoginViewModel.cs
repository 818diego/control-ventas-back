using System.ComponentModel.DataAnnotations;

namespace Presentación.Models.Usuario.Usuario
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; }=string.Empty;
    }
}
