using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class Ingreso
    {
        [Key]
        public int IdIngreso { get; set; }
        [Required]
        [ForeignKey("IdPersona")]
        public int IdPersona { get; set; }
        [Required]
        [ForeignKey("IdUsuario")]
        public int IdUsuario { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150.")]
        public string TipoComprobante { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        public string SerieComprobante { get; set; } = string.Empty;
        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150.")]
        public string NumeroComprobante { get; set; } = string.Empty;
        [Required]
        public DateTime FechaHoraIngreso{ get; set; }
        [Required]
        public decimal Impuestos { get; set; }
        [Required]
        public decimal Total { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "El nombre debe tener menos de tres caracteres, ni mas de 150.")]
        public bool Estado { get; set; }
        public Persona Persona { get; set; }
        public Usuario Usuario{ get; set; }
        [ForeignKey("IdIngreso")]
        public ICollection<DetalleIngreso> DetalleIngreso { get; set; }
    }
}
