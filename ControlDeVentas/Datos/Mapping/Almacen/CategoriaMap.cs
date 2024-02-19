using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Entidades.Almacen;

namespace Datos.Mapping.Almacen
{
    internal class CategoriaMap : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("Categorias").HasKey(c => c.IdCategoria);
            builder.Property(c => c.NombreCategoria).HasMaxLength(50);
            builder.Property(c => c.Descripcion).HasMaxLength(250);
            builder.Property(c=>c.Estado).HasDefaultValue(false);
        }
    }
}
