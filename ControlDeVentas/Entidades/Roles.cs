using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Roles
    {
        public int IdRol { get; set; }
        [Required]
        [StringLength(30)]
        public string NombreRol { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string DescripcionRol { get; set; } = string.Empty;
        public bool Estado { get; set; }

        [ForeignKey("IdRol")]
        public ICollection<Usuario> Usuarios { get; set; }
    }
}
