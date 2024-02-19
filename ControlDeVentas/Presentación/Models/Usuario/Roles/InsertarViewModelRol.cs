﻿using System.ComponentModel.DataAnnotations;

namespace Presentación.Models.Usuario.Roles
{
    public class InsertarViewModelRol
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El nombre del rol no debe de tener menos de 3 caracteres, ni más de 30")]
        public string NombreRol { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string DescripcionRol { get; set; } = string.Empty;
        public bool Estado { get; set; }
    }
}
