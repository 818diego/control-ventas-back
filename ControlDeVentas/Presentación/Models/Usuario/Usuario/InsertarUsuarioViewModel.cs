using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace Presentación.Models.Usuario.Usuario
{
    public class InsertarUsuarioViewModel
    {
        [System.ComponentModel.DataAnnotations.Required]
        public int IdRol { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150.")]
        public string NombreUsuario { get; set; } = string.Empty;
        public string TipoDocumento { get; set; }= string.Empty;
        public string NumeroDocumento { get; set; }= string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        [System.ComponentModel.DataAnnotations.Required]
        [EmailAddress]
        public string Email { get; set; }=string.Empty;
        [System.ComponentModel.DataAnnotations.Required]
        public string Password { get; set; } = string.Empty;
    }
}
