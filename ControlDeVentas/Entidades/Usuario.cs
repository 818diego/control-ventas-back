using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        [Required]
        [ForeignKey("IdRol")]
        public int IdRol { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150.")]
        public string NombreUsuario { get; set; } = string.Empty;
        [StringLength(20)]
        public string TipoDocumento { get; set; } = string.Empty;
        [StringLength(20)]
        public string NumeroDocumento { get; set; } = string.Empty;
        [StringLength(150)]
        public string Direccion { get; set; } = string.Empty;
        [StringLength(14)]
        public string Telefono { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public byte[] PasswordHash { get; set; }
        [Required]
        public byte[] PasswordSalt { get; set; }
        public bool Estado { get; set; }=false;
        public Roles Rol { get; set; }
        [ForeignKey("IdUsuario")]
        public ICollection<Venta> Venta { get; set; }
        [ForeignKey("IdUsuario")]
        public ICollection<Ingreso> Ingreso { get; set; }

    }
}
