using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades;
using Presentación.Models.Venta.Ingresos;
using Presentación.Models.Venta.DetalleIngreos;
using Presentación.Models.Venta.Venta;

namespace Presentación.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public IngresosController(DBContextSistema context)
        {
            _context = context;
        }

        #region Listar. Get: api/Ingresos/ListarIngresos
        [HttpGet("[action]")]
        public async Task<IEnumerable<ListandoIngresoViewModel>> ListarIngresos()
        {
            var ingreso = await _context.Ingresos.Include(u => u.Persona).Include(u => u.Usuario).OrderByDescending(i => i.IdIngreso).Take(100).ToListAsync();
            return ingreso.Select(u => new ListandoIngresoViewModel
            {
                IdIngreso = u.IdIngreso,
                IdUsuario = u.IdUsuario,
                IdPersona = u.IdPersona,
                NombreUsuario=u.Usuario.NombreUsuario,
                NombreProvedor=u.Persona.NombrePersona,
                TipoComprobante = u.TipoComprobante,
                SerieComprobante = u.SerieComprobante,
                NumeroComprobante = u.NumeroComprobante,
                FechaHoraIngreso = u.FechaHoraIngreso,
                Impuestos = u.Impuestos,
                Total = u.Total,
                Estado = u.Estado
            });

        }
        #endregion

        #region Listar. Get: api/Ingresos/ListadoFiltrado
        [HttpGet("[action]/{criterio}")]
        public async Task<IEnumerable<ListandoIngresoViewModel>> ListadoFiltrado([FromRoute] string criterio)
        {
            var ingreso = await _context.Ingresos.Include(u => u.Usuario).Include(u => u.Persona).Where(i => i.NumeroComprobante.Contains(criterio)).OrderByDescending(i => i.IdIngreso).ToListAsync();
            return ingreso.Select(u => new ListandoIngresoViewModel
            {
                IdIngreso = u.IdIngreso,
                IdUsuario = u.IdUsuario,
                IdPersona = u.IdPersona,
                NombreUsuario = u.Usuario.NombreUsuario,
                NombreProvedor = u.Persona.NombrePersona,
                TipoComprobante = u.TipoComprobante,
                SerieComprobante = u.SerieComprobante,
                NumeroComprobante = u.NumeroComprobante,
                FechaHoraIngreso = u.FechaHoraIngreso,
                Impuestos = u.Impuestos,
                Total = u.Total,
                Estado = u.Estado
            });

        }
        #endregion
       
        #region Listar.Get : api/Ingresos/ListarDetalleIngresos
        [HttpGet("[action]/{idIngreso}")]
        public async Task<IEnumerable<DetalleIngresosViewModel>> ListarDetalleIngreso([FromRoute] int idIngreso)
        {
            var detalle = await _context.DetalleIngresos.Include(u => u.Articulo).Where(u => u.IdIngreso == idIngreso).ToListAsync();
            return detalle.Select(u => new DetalleIngresosViewModel
            {
                IdArticulo = u.IdArticulo,
                Cantidad = u.Cantidad,
                NombreArticulo = u.Articulo.NombreArticulo,
                Precio = u.Precio,

            });
        }
        #endregion

        #region Insertar. POST api/Ingresos/InsertarIngreso
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarIngreso(InsertarIngresoModel modelIngreso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.Ingresos == null)
            {
                return Problem("Entity set 'BdContextSistema.Venta' is nulpl");
            }
            DateTime fechaHora = DateTime.Now;
            Ingreso ingreso = new()
            {
                IdPersona = modelIngreso.IdPersona,
                IdUsuario = modelIngreso.IdUsuario,
                TipoComprobante = modelIngreso.TipoComprobante,
                SerieComprobante = modelIngreso.SerieComprobante,
                NumeroComprobante = modelIngreso.NumeroComprobante,
                FechaHoraIngreso = fechaHora,
                Impuestos = modelIngreso.Impuestos,
                Total = modelIngreso.Total,
                Estado = true,
            };
            
            try
            {
                _context.Ingresos.Add(ingreso);
                await _context.SaveChangesAsync();

                var idIngreso = ingreso.IdIngreso;
                foreach(var det in modelIngreso.Detalles)
                {
                    DetalleIngreso detalle=new DetalleIngreso()
                    {
                        IdIngreso=idIngreso,
                        IdArticulo=det.IdArticulo,
                        Cantidad=det.Cantidad,
                        Precio=det.Precio

                    };
                    _context.DetalleIngresos.Add(detalle);
                }
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                string Error = e.Message;
                var inner = e.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion 

        #region Activar. PUT: api/Ingresos/DesactivarIngresos/5
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarIngreso([FromRoute]int id)
        {
            if(id<0)
            {
                return BadRequest();
            }
            var ingreso = await _context.Ingresos.FirstOrDefaultAsync(i=>i.IdIngreso==id);
            if (ingreso == null)
            {
                return NotFound();
            }
            ingreso.Estado = false;

            try
            {
                await _context.SaveChangesAsync();
                var detalles=await _context.DetalleIngresos.Include(a=>a.Articulo).Where(d=>d.IdIngreso==id).ToListAsync();
                foreach(var d in detalles)
                {
                    var articulo = await _context.Articulo.FirstOrDefaultAsync(a=>a.IdArticulo==d.Articulo.IdArticulo);
                    articulo.Stock = d.Articulo.Stock - d.Cantidad;
                    await _context.SaveChangesAsync();
                }
            }catch( DbUpdateConcurrencyException) 
            {
                return BadRequest();
            }
            return Ok();
        }
        #endregion
        #region Listar. GET: api/Ingresos/ListadoIngresoRangoFechas/FechaInicial/FechaFinal
        [HttpGet("[action]/{FechaInicial}/{FechaFinal}")]
        public async Task<IEnumerable<ListandoIngresoViewModel>> ListadoRangoFechas([FromRoute] DateTime FechaInicial, DateTime FechaFinal)
        {
            var venta = await _context.Ingresos.Include(v => v.Usuario).Include(v => v.Persona).Where(v => v.FechaHoraIngreso > FechaInicial).Where(v => v.FechaHoraIngreso < FechaFinal).OrderByDescending(v => v.IdIngreso).ToListAsync();
            return venta.Select(u => new ListandoIngresoViewModel
            {
                IdIngreso = u.IdIngreso,
                IdUsuario = u.IdUsuario,
                IdPersona = u.IdPersona,
                NombreUsuario = u.Usuario.NombreUsuario,
                NombreProvedor = u.Persona.NombrePersona,
                TipoComprobante = u.TipoComprobante,
                SerieComprobante = u.SerieComprobante,
                NumeroComprobante = u.NumeroComprobante,
                FechaHoraIngreso = u.FechaHoraIngreso,
                Impuestos = u.Impuestos,
                Total = u.Total,
                Estado = u.Estado

            });
        }
        #endregion

        #region Listar. GET: api/Ingresos/GraficaIngresos
        [HttpGet("[action]")]
        public async Task<IEnumerable<GraficasIngresosViewModel>> GraficaIngresos()
        {
            var consulta = await _context.Ingresos.GroupBy(v => v.FechaHoraIngreso.Month).Select(x => new { etiqueta = x.Key, Ingreso = x.Sum(v => v.Total) }).OrderBy(v => v.etiqueta).ToListAsync();
            return consulta.Select(v => new GraficasIngresosViewModel
            {
                nombreMeses = v.etiqueta.ToString(),
                mesesValores = v.Ingreso
            });
        }
        #endregion


    }
}
