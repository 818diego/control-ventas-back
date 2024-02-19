using System.ComponentModel.DataAnnotations;

namespace Presentación.Models.Usuario.Roles
{
    public class RolesViewModel
    {
        public int IdRol { get; set; }
        public string NombreRol { get; set; } = string.Empty;
        public string DescripcionRol { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
