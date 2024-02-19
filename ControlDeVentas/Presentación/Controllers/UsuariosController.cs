using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Datos;
using Entidades;
using Presentación.Models.Usuario.Usuario;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Presentación.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly DBContextSistema _context;
        private readonly IConfiguration _configuracion;

        public UsuariosController(DBContextSistema context, IConfiguration configuracion)
        {
            _context = context;
            _configuracion = configuracion;
        }
        private bool VerificaPassword(string password, byte[] passwordHash, byte[]passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var nuevoPasswordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHash).SequenceEqual(new ReadOnlySpan<byte>(nuevoPasswordHash));
            }
        }
        
        private string GenerarTokens(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuracion["Jwt:Key"]));
            var credenciales =new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuracion["Jnt:Issuer"],
                _configuracion["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credenciales,
                claims:claims
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var email = model.Email.ToUpper();
            var usuario=await _context.Usuarios.Where(u=>u.Estado==true).Include(u=>u.Rol).FirstOrDefaultAsync(u=>u.Email==email);
            if (usuario==null) 
            {
                return NotFound();
            }
            var IsValido = VerificaPassword(model.Password, usuario.PasswordHash, usuario.PasswordSalt);
            if (!IsValido) 
            {
                return BadRequest();
            }
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email,email),
                new Claim(ClaimTypes.Role,usuario.Rol.NombreRol),

                new Claim("IdUsuario",usuario.IdUsuario.ToString()),
                new Claim("Rol",usuario.Rol.NombreRol),
                new Claim("NombreUsuario",usuario.NombreUsuario)
            };
            return Ok(
                new {token=GenerarTokens(claim)});
        }
        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
          if (_context.Usuarios == null)
          {
              return NotFound();
          }
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
          if (_context.Usuarios == null)
          {
              return Problem("Entity set 'DBContextSistema.Usuarios'  is null.");
          }
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUsuario", new { id = usuario.IdUsuario }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            if (_context.Usuarios == null)
            {
                return NotFound();
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return (_context.Usuarios?.Any(e => e.IdUsuario == id)).GetValueOrDefault();
        }

        #region Listar. GET: api/Usuarios/ListarUsuario
        [HttpGet("[action]")]
        public async Task<IEnumerable<UsuarioViewModel>> ListarUsuarios()
        {
            var articulo = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
            return articulo.Select(u => new UsuarioViewModel
            {
                IdUsuario = u.IdUsuario,
                IdRol = u.IdRol,
                NombreUsuario = u.NombreUsuario,
                TipoDocumento = u.TipoDocumento,
                NumeroDocumento = u.NumeroDocumento,
                Direccion = u.Direccion,
                Telefono = u.Telefono,
                Email = u.Email,
                PasswordHash = u.PasswordHash,
                Estado = u.Estado,
                Rol=u.Rol.NombreRol        
            });
        }
        #endregion
        #region Insertar. POST: api/Usuario/InsertarUsuario/5
        [HttpPost("[action]")]
        public async Task<IActionResult> InsertarUsuario(InsertarUsuarioViewModel modelUsuario)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(_context.Usuarios==null)
            {
                return Problem("Entity set 'BdContextSistema.Usuarios' is nulpl");

            }
            CreaPasswordHash(modelUsuario.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var email = modelUsuario.Email.ToUpper();
            if(await _context.Usuarios.AnyAsync(u=>u.Email==email))
            {
                return BadRequest("El Email del usuario ya existe");
            }
            Usuario usuario = new()
            {

                IdRol = modelUsuario.IdRol,
                NombreUsuario = modelUsuario.NombreUsuario,
                TipoDocumento = modelUsuario.TipoDocumento,
                NumeroDocumento = modelUsuario.NumeroDocumento,
                Direccion = modelUsuario.Direccion,
                Telefono = modelUsuario.Telefono,
                Email = modelUsuario.Email.ToUpper(),
                PasswordHash = passwordHash,
                Estado = true,
                PasswordSalt = passwordSalt

            };
            _context.Usuarios.Add(usuario);
            try
            {
                await _context.SaveChangesAsync();

            }
            catch(Exception e)
            {
                string Error = e.Message;
                var inner = e.InnerException;
                return BadRequest();
            }
            return Ok();
        }
        #endregion
        #region Modificar. PUT: api/Usuario/ModificarUsuario/5
        [HttpPut("[action]")]
        public async Task<IActionResult> ModificarUsuario([FromBody] ModificarUsuarioViewModel modelUsuario)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(modelUsuario.IdUsuario<0)
            {
                return BadRequest();
            }
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == modelUsuario.IdUsuario);
            var email = modelUsuario.Email.ToUpper();
            if(await _context.Usuarios.AllAsync(u=>u.Email==email && u.IdUsuario!=modelUsuario.IdUsuario))
            {
                return BadRequest("El Email del usuario ya existe");
            }
            if(usuario==null)
            {
                return NotFound();
            }

            usuario.IdUsuario = modelUsuario.IdUsuario;
            usuario.IdRol = modelUsuario.IdRol;
            usuario.NombreUsuario = modelUsuario.NombreUsuario;
            usuario.TipoDocumento = modelUsuario.TipoDocumento;
            usuario.NumeroDocumento = modelUsuario.NumeroDocumento;
            usuario.Direccion = modelUsuario.Direccion;
            usuario.Telefono = modelUsuario.Telefono;
            usuario.Email = modelUsuario.Email.ToUpper();

            if (modelUsuario.ActualizaPassword==true)
            {
                CreaPasswordHash(modelUsuario.Password, out byte[] passwordHash, out byte[] passwordSalt);
                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> DesactivarUsuario([FromRoute] int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var rol = await _context.Usuarios.FirstOrDefaultAsync(c => c.IdUsuario == id);

            if (rol == null)
            {
                return NotFound();
            }

            rol.Estado = false;

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


        #region ACTIVAR Usuario

        // ACTIVAR Usuario//
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> ActivarUsuario([FromRoute] int id)
        {
            if (id < 0)
            {
                return BadRequest();
            }

            var rol = await _context.Usuarios.FirstOrDefaultAsync(c => c.IdUsuario == id);

            if (rol == null)
            {
                return NotFound();
            }

            rol.Estado = true;

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

        #endregion
        private static void CreaPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
    
}
