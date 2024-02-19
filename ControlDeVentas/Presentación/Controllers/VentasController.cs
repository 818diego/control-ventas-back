using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades;
using Presentación.Models.Venta.Venta;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;
using Presentación.Models.Venta.Ingresos;
using Presentación.Models.Venta.DetalleIngreos;
using Presentación.Models.Venta.DatalleVenta;

namespace Presentación.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public VentasController(DBContextSistema context)
        {
            _context = context;
        }

        #region Listar. Get : api/Ventas/ListarVentas
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> ListarVentas()
        {
            var venta = await _context.Ventas.Include(i=>i.Usuario).Include(i=>i.Persona).OrderByDescending(i=>i.IdVenta).Take(100).ToListAsync();
            return venta.Select(u => new VentaViewModel
            {
                IdVenta=u.IdVenta,
                IdPersona=u.IdPersona,
                IdUsuario=u.IdUsuario,
                NombreUsuario=u.Usuario.NombreUsuario,
                NombreCliente=u.Persona.NombrePersona,
                TipoComprobante =u.TipoComprobante,
                SerieComprobante=u.SerieComprobante,
                NumeroComprobante=u.NumeroComprobante,
                FechaHora=u.FechaHora,
                Impuesto=u.Impuesto,
                Total=u.Total, 
                Estado=u.Estado
                
            });
        }
        #endregion
        #region Listar. Get : api/Ventas/ListadoFiltrado
        [HttpGet("[action]")]
        public async Task<IEnumerable<VentaViewModel>> ListadoFiltrado([FromRoute]string criterio)
        {
            var venta = await _context.Ventas.Include(i => i.Usuario).Include(i => i.Persona).Where(i=>i.NumeroComprobante.Contains(criterio)).OrderByDescending(i=>i.IdVenta).ToListAsync();
            return venta.Select(u => new VentaViewModel
            {
                IdVenta = u.IdVenta,
                IdPersona = u.IdPersona,
                IdUsuario = u.IdUsuario,
                NombreUsuario = u.Usuario.NombreUsuario,
                NombreCliente = u.Persona.NombrePersona,
                TipoComprobante = u.TipoComprobante,
                SerieComprobante = u.SerieComprobante,
                NumeroComprobante = u.NumeroComprobante,
                FechaHora = u.FechaHora,
                Impuesto = u.Impuesto,
                Total = u.Total,
                Estado = u.Estado

            });
        }
        #endregion

        #region Insertar. Post: api/Ventas/InsertarVenta
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarVenta(InsertarVentaModel modelVenta)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            if(_context.Ventas==null)
            {
                return Problem("Entity set 'BdContextSistema.Venta' is nulpl");
            }
            DateTime FechaHora=DateTime.Now;
            Venta venta = new Venta
            {
                IdPersona = modelVenta.IdPersona,
                IdUsuario = modelVenta.IdUsuario,
                TipoComprobante = modelVenta.TipoComprobante,
                SerieComprobante = modelVenta.SerieComprobante,
                NumeroComprobante = modelVenta.NumeroComprobante,
                FechaHora = FechaHora,
                Impuesto = modelVenta.Impuesto,
                Total = modelVenta.Total,
                Estado = true
            };
            
            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                var idVenta = venta.IdVenta;
                foreach(var det in modelVenta.Detalles) 
                {
                    DetalleVenta detalle = new DetalleVenta()
                    {
                        IdVenta = idVenta,
                        IdArticulo = det.IdArticulo,
                        Cantidad = det.cantidad,
                        Descuento = det.descuento,
                    };
                    _context.DetalleVentas.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch(Exception e) 
            {
                string Error= e.Message;
                var inner = e.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion


        #region Desactivar Venta
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarVenta([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var venta = await _context.Ventas.FirstOrDefaultAsync(c => c.IdVenta == id);

            if (venta == null)
            {
                return NotFound();
            }

            venta.Estado = false;

            try
            {
                await _context.SaveChangesAsync();
                var detalles=await _context.DetalleVentas.Include(a=>a.Articulo).Where(d=>d.IdVenta==id).ToListAsync();

                foreach( var d in detalles)
                {
                    var articulo = await _context.Articulo.FirstOrDefaultAsync(a => a.IdArticulo == d.Articulo.IdArticulo);
                    articulo.Stock=d.Articulo.Stock+d.Cantidad;
                    await _context.SaveChangesAsync();

                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }
        #endregion
        #region Listar.Get : api/Vantas/ListarDetalleIngresos
        [HttpGet("[action]/{idVenta}")]
        public async Task<IEnumerable<DetalleVentaViewModel>> ListarDetalleVenta([FromRoute] int idVenta)
        {
            var detalle = await _context.DetalleVentas.Include(u => u.Articulo).Where(u => u.IdVenta == idVenta).ToListAsync();
            return detalle.Select(u => new DetalleVentaViewModel
            {
                IdArticulo = u.IdArticulo,
                cantidad = u.Cantidad,
                nombreArticulo = u.Articulo.NombreArticulo,
                precio = u.Articulo.PrecioVenta

            });
        }
        #endregion

        #region Listar. GET: api/Ventas/ListadoVentasRangoFechas/FechaInicial/FechaFinal
        [HttpGet("[action]/{FechaInicial}/{FechaFinal}")]
        public async Task<IEnumerable<VentaViewModel>> ListadoRangoFechas([FromRoute] DateTime FechaInicial, DateTime FechaFinal)
        {
            var venta = await _context.Ventas.Include(v => v.Usuario).Include(v => v.Persona).Where(v => v.FechaHora > FechaInicial).Where(v => v.FechaHora < FechaFinal).OrderByDescending(v => v.IdVenta).ToListAsync();
            return venta.Select(u => new VentaViewModel
            {
                IdVenta = u.IdVenta,
                IdPersona = u.IdPersona,
                IdUsuario = u.IdUsuario,
                NombreUsuario = u.Usuario.NombreUsuario,
                NombreCliente = u.Persona.NombrePersona,
                TipoComprobante = u.TipoComprobante,
                SerieComprobante = u.SerieComprobante,
                NumeroComprobante = u.NumeroComprobante,
                FechaHora = u.FechaHora,
                Impuesto = u.Impuesto,
                Total = u.Total,
                Estado = u.Estado

            });
        }

        #endregion

        #region Listar. GET: api/Ventas/GraficaVentas
        [HttpGet("[action]")]
        public async Task<IEnumerable<GraficaVentasViewModel>> GraficaVentas()
        {
            var consulta = await _context.Ventas.GroupBy(v => v.FechaHora.Month).Select(x => new { etiqueta = x.Key, ventas = x.Sum(v => v.Total) }).OrderBy(v => v.etiqueta).ToListAsync();
            return consulta.Select(v => new GraficaVentasViewModel
            {
                nombreMeses=v.etiqueta.ToString(),
                mesesValores=v.ventas
            });
        }
        #endregion
    }
}
