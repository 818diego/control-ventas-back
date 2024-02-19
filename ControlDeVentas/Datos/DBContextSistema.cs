using Datos.Mapping;
using Datos.Mapping.Almacen;
using Datos.Mapping.Usuarios;
using Datos.Mapping.Ventas;
using Entidades;
using Entidades.Almacen;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datos
{
    public class DBContextSistema: DbContext
    {
        public DbSet<Categoria> Categorias { get; set; } = null!;
        public DbSet<Roles> Roles { get; set; } = null!;
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Ingreso> Ingresos { get; set; }
        public DbSet<DetalleIngreso> DetalleIngresos { get; set; }
        public DBContextSistema() { }
        public DBContextSistema(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Conexion");
            }
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DetalleIngreso>().ToTable(tb => tb.HasTrigger("ActualizarStock_Ingreso"));
            modelBuilder.Entity<DetalleVenta>().ToTable(tb => tb.HasTrigger("ActualizarStock_Venta"));
            modelBuilder.ApplyConfiguration(new CategoriaMap());
            modelBuilder.ApplyConfiguration(new RolesMap());
            modelBuilder.ApplyConfiguration(new ArticuloMap());
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PersonaMap());
            modelBuilder.ApplyConfiguration(new VentaMap());
            modelBuilder.ApplyConfiguration(new DetalleVentaMap());
            modelBuilder.ApplyConfiguration(new IngresoMap());
            modelBuilder.ApplyConfiguration(new DetalleIngresoMap());
        }
   
    }
}
