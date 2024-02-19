using Entidades;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Mapping.Almacen
{
    public class ArticuloMap : IEntityTypeConfiguration<Articulo>
    {
        public void Configure(EntityTypeBuilder<Articulo> builder)
        {
            builder.ToTable("Articulos").HasKey(a => a.IdArticulo);

            builder.HasOne(c => c.Categoria)
                .WithMany(a => a.Articulos)
                .HasForeignKey(d => d.IdCategoria);
            builder.Property(a => a.CodigoArticulo).HasMaxLength(50);
            builder.Property(a => a.NombreArticulo).HasMaxLength(150);
            builder.Property(a => a.PrecioVenta).HasColumnType("decimal(10, 2)");
            builder.Property(a => a.Stock);
            builder.Property(a => a.DescripcionArticulo).HasMaxLength(250);
            builder.Property(c => c.Estado).HasDefaultValue(false);

        }
    }
}
