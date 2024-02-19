using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades;
using Presentación.Models.Usuario.Personas;
using Presentación.Models.Venta.Venta;

namespace Presentación.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DBContextSistema _context;

        public PersonasController(DBContextSistema context)
        {
            _context = context;
        }

        #region Insertar. POST: api/Personas/InsertarPersona
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarPersona(PersonaViewModel modelPersona)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                
            }
            if(_context.Persona == null) 
            {
                return Problem("Entity set 'Context.persona is null");
            }
            var email= modelPersona.EmailPersona.ToUpper();
            if(await _context.Persona.AnyAsync(p=>p.EmailPersona== email))
            {
                return BadRequest("El Email de la persona ya existe");
            }
            Persona persona = new()
            {
                TipoPersona = modelPersona.TipoPersona,
                NombrePersona = modelPersona.NombrePersona,
                Tipodocumento = modelPersona.Tipodocumento,
                NumeroDocumento = modelPersona.NumeroDocumento,
                DireccionPersona = modelPersona.DireccionPersona,
                TelefonoPersona = modelPersona.TelefonoPersona,
                EmailPersona = modelPersona.EmailPersona.ToUpper(),
            };
            _context.Persona.Add(persona);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex) 
            {
                string Error=ex.Message;
                var inner = ex.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion

        #region Modificar. PUT: api/Personas/ModificarPersona
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarPersona(ModificarPersonaViewModel modelPersona)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            if (modelPersona.IdPersona <=0)
            {
                return Problem("Entity set 'Context.persona is null");
            }
            var persona = await _context.Persona.FirstOrDefaultAsync(p=> p.IdPersona== modelPersona.IdPersona);
            var email = modelPersona.EmailPersona.ToUpper();
            if (await _context.Persona.AnyAsync(p => p.EmailPersona == email && p.IdPersona!=modelPersona.IdPersona))
            {
                return BadRequest("El Email de la persona ya existe");
            }
            if (persona == null)
            {
                return NotFound();
            }

            persona.IdPersona= modelPersona.IdPersona;
            persona.TipoPersona = modelPersona.TipoPersona;
            persona.NombrePersona = modelPersona.NombrePersona;
            persona.Tipodocumento = modelPersona.Tipodocumento;
            persona.NumeroDocumento = modelPersona.NumeroDocumento;
            persona.DireccionPersona = modelPersona.DireccionPersona;
            persona.TelefonoPersona = modelPersona.TelefonoPersona;
            persona.EmailPersona = modelPersona.EmailPersona.ToUpper();
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string Error = ex.Message;
                var inner = ex.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion

        #region Listar. GET: api/Personas/ListarClientes
        [HttpGet("[action]")]
        public async Task<IEnumerable<ModificarPersonaViewModel>> ListarCliente()
        {
            var persona= await _context.Persona.Where(p=>p.TipoPersona=="Cliente").ToListAsync();
            return persona.Select(p => new ModificarPersonaViewModel
            {
                IdPersona = p.IdPersona,
                TipoPersona = p.TipoPersona,
                NombrePersona = p.NombrePersona,
                Tipodocumento = p.Tipodocumento,
                NumeroDocumento = p.NumeroDocumento,
                DireccionPersona = p.DireccionPersona,
                TelefonoPersona = p.TelefonoPersona,
                EmailPersona = p.EmailPersona
            });
            
        }
        #endregion

        #region Listar. GET: api/Personas/ListarProvedores
        [HttpGet("[action]")]
        public async Task<IEnumerable<ModificarPersonaViewModel>> ListarProvedores()
        {
            var persona = await _context.Persona.Where(p => p.TipoPersona == "Proveedor").ToListAsync();
            return persona.Select(p => new ModificarPersonaViewModel
            {
                IdPersona = p.IdPersona,
                TipoPersona = p.TipoPersona,
                NombrePersona = p.NombrePersona,
                Tipodocumento = p.Tipodocumento,
                NumeroDocumento = p.NumeroDocumento,
                DireccionPersona = p.DireccionPersona,
                TelefonoPersona = p.TelefonoPersona,
                EmailPersona = p.EmailPersona
            });

        }
        #endregion

        #region Seleccionar. GET: api/Personas/SeleccionarProveedor
        [HttpGet("[action]")]
        public async Task<IEnumerable<SeleccionaProvedorViewModel>> SeleccionaProveedor()
        {
            var persona = await _context.Persona.Where(p => p.TipoPersona == "Proveedor").ToListAsync();
            return persona.Select(c => new SeleccionaProvedorViewModel
            {
                IdPersona=c.IdPersona,
                NombrePersona=c.NombrePersona
            });
        }

        #endregion



        #region Seleccionar. GET: api/Personas/SeleccionarCliente
        [HttpGet("[action]")]
        public async Task<IEnumerable<SeleccionaProvedorViewModel>> SeleccionaCliente()
        {
            var persona = await _context.Persona.Where(p => p.TipoPersona == "Cliente").ToListAsync();
            return persona.Select(c => new SeleccionaProvedorViewModel
            {
                IdPersona = c.IdPersona,
                NombrePersona = c.NombrePersona
            });
        }

        #endregion
    }
}
