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
    internal class IngresoMap : IEntityTypeConfiguration<Ingreso>
    {
        public void Configure(EntityTypeBuilder<Ingreso> builder)
        {
            builder.ToTable("Ingresos").HasKey(u => u.IdIngreso);
            builder.HasOne(i=>i.Persona).WithMany(p=>p.Ingreso).HasForeignKey(i=>i.IdPersona);
            builder.HasOne(i=>i.Usuario).WithMany(u=>u.Ingreso).HasForeignKey(u=>u.IdUsuario);
        }
    }
}
