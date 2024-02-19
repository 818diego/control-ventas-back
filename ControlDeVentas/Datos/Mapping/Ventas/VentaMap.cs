using Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos.Mapping.Ventas
{
    internal class VentaMap : IEntityTypeConfiguration<Venta>
    {
        public void Configure(EntityTypeBuilder<Venta> builder)
        {
            builder.ToTable("Ventas").HasKey(u => u.IdVenta);
            builder.HasOne(i => i.Persona).WithMany(p => p.Venta).HasForeignKey(i => i.IdPersona);
            builder.HasOne(i => i.Usuario).WithMany(u => u.Venta).HasForeignKey(u => u.IdUsuario);
        }
    }
}
