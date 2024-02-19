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
using Presentación.Models.Venta.DetalleIngreos;

namespace Presentación.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleIngresosController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public DetalleIngresosController(DBContextSistema context)
        {
            _context = context;
        }
       
        #region Insertar. Post: api/DetalleIngresos/InsertarDetalleIngreso 
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarDetalleIngreso(InsertarDetalleIngresoModel modelIngreso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.DetalleIngresos == null)
            {
                return Problem("Entity set 'BdContextSistema.Ingreso' is nulpl");
            }
            DetalleIngreso detalle = new()
            {
                IdIngreso = modelIngreso.IdIngreso,
                IdArticulo = modelIngreso.IdArticulo,
                Cantidad = modelIngreso.Cantidad,
                Precio = modelIngreso.Precio

            };
            _context.DetalleIngresos.Add(detalle);

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
        #region Modificar. Put: api/DetalleIngresos/ModificarDetalleIngreso/5
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarDetalleIngreso([FromBody] ModificarDetalleIngresoModel modelDetalle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (modelDetalle.IdDetalleIngreso < 0)
            {
                return BadRequest();
            }
            var detalle = await _context.DetalleIngresos.FirstOrDefaultAsync(u => u.IdDetalleIngreso == modelDetalle.IdDetalleIngreso);
            if (detalle == null)
            {
                return NotFound();
            }
            detalle.IdDetalleIngreso = modelDetalle.IdDetalleIngreso;
            detalle.IdIngreso = modelDetalle.IdIngreso;
            detalle.IdArticulo = modelDetalle.IdArticulo;
            detalle.Cantidad = modelDetalle.Cantidad;
            detalle.Precio = modelDetalle.Precio;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return Ok();
        }
        #endregion
        #region Insertar. Post: api/DetalleIngresos/InsertarDetalleIngresoLista 
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarDetalleIngresoLista(IEnumerable<InsertarDetalleIngresoModel> modelIngreso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_context.DetalleIngresos == null)
            {
                return Problem("Entity set 'BdContextSistema.Ingreso' is nulpl");
            }
            foreach (var modelDetalle in modelIngreso)
            {
                DetalleIngreso detalle = new()
                {
                    IdIngreso = modelDetalle.IdIngreso,
                    IdArticulo = modelDetalle.IdArticulo,
                    Cantidad = modelDetalle.Cantidad,
                    Precio = modelDetalle.Precio

                };
                _context.DetalleIngresos.Add(detalle);
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
