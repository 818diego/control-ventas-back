using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades;
using Presentación.Models.Almacen.Articulo;

namespace Presentación.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public ArticulosController(DBContextSistema context)
        {
            _context = context;
        }

        #region obtener. Get: api/Articulos/ObtenerArticulos/3
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> ObtenerArticulo([FromRoute] int id)
        {
            var articulo = await _context.Articulo.Include(a=>a.Categoria).SingleOrDefaultAsync(a=>a.IdArticulo==id);
            if(articulo==null)
            {
                return NotFound();
            }
            return Ok(new ArticuloViewModel
                {
                    IdArticulo = articulo.IdArticulo,
                    IdCategoria = articulo.IdCategoria,
                    CodigoArticulo = articulo.CodigoArticulo,
                    NombreArticulo =articulo.NombreArticulo,
                    DescripcionArticulo=articulo.DescripcionArticulo,
                    Stock= articulo.Stock,
                    PrecioVenta= articulo.PrecioVenta,
                    Estado= articulo.Estado,

                    Categoria=articulo.Categoria.NombreCategoria
                });
        }
        #endregion

        #region Listar. Get: api/Articulos/ListarArticulos
        [HttpGet("[action]")]
        public async Task<IEnumerable<ArticuloViewModel>> ListarArticulo()
        {
            var articulo = await _context.Articulo.Include(a => a.Categoria).ToListAsync();
            return articulo.Select(a=>new ArticuloViewModel
            {
                IdArticulo = a.IdArticulo,
                IdCategoria = a.IdCategoria,
                CodigoArticulo = a.CodigoArticulo,
                NombreArticulo = a.NombreArticulo,
                DescripcionArticulo = a.DescripcionArticulo,
                Stock = a.Stock,
                PrecioVenta = a.PrecioVenta,
                Estado = a.Estado,

                Categoria = a.Categoria.NombreCategoria
            });
        }
        #endregion

        #region Listado por criterio. GET: api/Articulos/ListarIngresos/criterio
        [HttpGet("[action]/{criterio}")]
        public async Task<IEnumerable<ArticuloViewModel>> ListadoArticulos([FromRoute] string criterio)
        {
            var articulo = await _context.Articulo.Include(a=>a.Categoria).Where(a=>a.NombreArticulo.Contains(criterio)).Where(a=>a.Estado==true).ToListAsync();
            return articulo.Select(a => new ArticuloViewModel
            {
                IdArticulo=a.IdArticulo,
                IdCategoria=a.IdCategoria,
                CodigoArticulo=a.CodigoArticulo,
                NombreArticulo=a.NombreArticulo,
                PrecioVenta=a.PrecioVenta,
                Stock=a.Stock,
                DescripcionArticulo=a.DescripcionArticulo,
                Estado=a.Estado,
                Categoria=a.Categoria.NombreCategoria
            });
        }
        #endregion

        #region Obtener articulo por codigo. GET: api/Articulos/ObetenerArticuloPorCodigo/3
        [HttpGet("[action]/{codigo}")]
        public async Task<IActionResult> BuscarArticuloPorCodigo([FromRoute] string codigo)
        {
            var articulo = await _context.Articulo.Include(a=>a.Categoria).Where(a=>a.Estado== true).SingleOrDefaultAsync(a=>a.CodigoArticulo==codigo);
            if(articulo==null)
            {
                return NotFound();
            }
            return Ok(new ArticuloViewModel
            {
                IdArticulo=articulo.IdArticulo,
                IdCategoria=articulo.IdCategoria,
                CodigoArticulo=articulo.CodigoArticulo,
                NombreArticulo=articulo.NombreArticulo,
                Stock=articulo.Stock,
                PrecioVenta=articulo.PrecioVenta,
                Categoria=articulo.Categoria.NombreCategoria
            });
        }

        #endregion

        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarArticulos([FromBody] ModificarArticuloViewModel modelArticulo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (modelArticulo.IdArticulo < 0)
            {
                return BadRequest(ModelState);
            }

            var articulo = await _context.Articulo.FirstOrDefaultAsync(a => a.IdArticulo == modelArticulo.IdArticulo);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.IdCategoria = modelArticulo.IdCategoria;
            articulo.CodigoArticulo = modelArticulo.CodigoArticulo;
            articulo.NombreArticulo = modelArticulo.NombreArticulo;
            articulo.PrecioVenta = modelArticulo.PrecioVenta;
            articulo.Stock = modelArticulo.Stock;
            articulo.DescripcionArticulo = modelArticulo.DescripcionArticulo;
            articulo.Estado = modelArticulo.Estado;

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

        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarArticulos(InsertarArticuloViewModel modelArticulo)
        {
            if (_context.Articulo == null)
            {
                return Problem("Entity set 'DBContextSistema.Roles'  is null.");
            }

            Articulo articulo = new Articulo
            {
                IdCategoria = modelArticulo.IdCategoria,
                CodigoArticulo = modelArticulo.CodigoArticulo,
                NombreArticulo = modelArticulo.NombreArticulo,
                PrecioVenta = modelArticulo.PrecioVenta,
                Stock = modelArticulo.Stock,
                DescripcionArticulo = modelArticulo.DescripcionArticulo,
                Estado = modelArticulo.Estado
            };
            _context.Articulo.Add(articulo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarArticulos([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulo.FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.Estado = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> ActivarArticulos([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var articulo = await _context.Articulo.FirstOrDefaultAsync(a => a.IdArticulo == id);

            if (articulo == null)
            {
                return NotFound();
            }

            articulo.Estado = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return NoContent();
        }

    }

}



