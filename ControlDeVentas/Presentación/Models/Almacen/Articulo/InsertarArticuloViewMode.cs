using System.ComponentModel.DataAnnotations;

namespace Presentación.Models.Almacen.Articulo
{
    public class InsertarArticuloViewModel
    {
        [Required]
        public int IdCategoria { get; set; }


        [StringLength(50, MinimumLength = 3, ErrorMessage = "El código no debe de tener menos de 3 caracteres, ni más de 50")]
        public string CodigoArticulo { get; set; } = string.Empty;
        [Required]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener menos de 3 caracteres, ni más de 150")]
        public string NombreArticulo { get; set; } = string.Empty;

        [Required]
        public decimal PrecioVenta { get; set; }
        [Required]
        public int Stock { get; set; }
        [StringLength(250, MinimumLength = 3, ErrorMessage = "El nombre no debe de tener menos de 3 caracteres, ni más de 150")]
        public string DescripcionArticulo { get; set; } = string.Empty;

        public bool Estado { get; set; }
    }
   
}
