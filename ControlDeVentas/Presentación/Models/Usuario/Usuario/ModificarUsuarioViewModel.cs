using System.ComponentModel.DataAnnotations.Schema;

namespace Presentación.Models.Usuario.Usuario
{
    public class ModificarUsuarioViewModel
    {
        public int IdUsuario { set; get; }
        public int IdRol { set; get; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string TipoDocumento { get; set; } = string.Empty;
        public string NumeroDocumento { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool ActualizaPassword { get; set; }= false;




    }
}
