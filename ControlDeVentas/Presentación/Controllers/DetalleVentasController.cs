using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades;
using Presentación.Models.Venta.DatalleVenta;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using System.Security.Policy;

namespace Presentación.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleVentasController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public DetalleVentasController(DBContextSistema context)
        {
            _context = context;
        }

        #region Listar.Get : api/DetalleVentas/ListarDetalleVentas
        [HttpGet("[action]")]   
        public async Task<IEnumerable<DetalleVentaViewModel>> ListarDetalleVenta()
        {
            var detalle=await _context.DetalleVentas.Include(u=>u.Articulo).Include(u=>u.Venta).ToListAsync();
            return detalle.Select(u => new DetalleVentaViewModel
            {
                IdVenta = u.IdVenta,
                IdArticulo = u.IdArticulo,
                cantidad = u.Cantidad,
                descuento = u.Descuento,
                precio=u.Articulo.PrecioVenta
            });
        }
        #endregion
        #region Insertar. Post: api/DetalleVentas/InsertarDetalleVenta 
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarDetalleVenta (InsertarDetalleVentaModel modelVenta)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(_context.DetalleVentas==null)
            {
                return Problem("Entity set 'BdContextSistema.Venta' is nulpl");
            }
            DetalleVenta detalle = new()
            {
                IdVenta=modelVenta.IdVenta,
                IdArticulo=modelVenta.IdArticulo,
                Cantidad=modelVenta.Cantidad,
                Descuento=modelVenta.Descuento

            };
            _context.DetalleVentas.Add(detalle);

            try 
            {
                await _context.SaveChangesAsync();
            }catch(Exception e) 
            {
                string Error = e.Message;
                var inner = e.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion
        #region Modificar. Put: api/DetalleVentas/ModificarDetalleVenta/5
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarDetalleVenta([FromBody] ModificarDetalleVentaModel modelDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (modelDetalle.IdDetalleVentas < 0)
            {
                return BadRequest();
            }
            var detalle = await _context.DetalleVentas.FirstOrDefaultAsync(u => u.IdDetalleVentas == modelDetalle.IdDetalleVentas);
            if (detalle == null)
            {
                return NotFound();
            }
            detalle.IdDetalleVentas = modelDetalle.IdDetalleVentas;
            detalle.IdVenta=modelDetalle.IdVenta;
            detalle.IdArticulo = modelDetalle.IdArticulo;
            detalle.Cantidad = modelDetalle.Cantidad; 
            detalle.Descuento=modelDetalle.Descuento;
            try
            {
                await _context.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException) 
            {
                return BadRequest();
            }
            return Ok();
    }
        #endregion
        #region Insertar. Post: api/DetalleVentas/InsertarDetalleVentaLista 
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarDetalleVentaLista(IEnumerable<InsertarDetalleVentaModel> modelVenta)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.DetalleVentas == null)
            {
                return Problem("Entity set 'BdContextSistema.Venta' is nulpl");
            }
            foreach(var modelDetalle in modelVenta)
            {
                DetalleVenta detalle = new()
                {
                    IdVenta = modelDetalle.IdVenta,
                    IdArticulo = modelDetalle.IdArticulo,
                    Cantidad = modelDetalle.Cantidad,
                    Descuento = modelDetalle.Descuento

                };
                _context.DetalleVentas.Add(detalle);
            }
            try
            {
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

    }
}
