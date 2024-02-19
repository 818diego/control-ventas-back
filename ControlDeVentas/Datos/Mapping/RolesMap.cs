using Entidades.Almacen;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;

namespace Datos.Mapping
{
    internal class RolesMap : IEntityTypeConfiguration<Roles>
    {

        public void Configure(EntityTypeBuilder<Roles> builder)
        {
            builder.ToTable("Roles").HasKey(c => c.IdRol);
            builder.Property(c => c.NombreRol).HasMaxLength(30);
            builder.Property(c => c.DescripcionRol).HasMaxLength(100);
            builder.Property(c => c.Estado).HasDefaultValue(false);
        }
    }
}
