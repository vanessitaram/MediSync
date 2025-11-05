using MediSync.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace MediSync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedicoController : ControllerBase
    {
        private readonly MediSyncContext _context;

        public MedicoController(MediSyncContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonElement cred)
        {
            try
            {
                string correo = cred.GetProperty("correomedico").GetString();
                string contrasena = cred.GetProperty("contrasena").GetString();

                Console.WriteLine($" Intentando login → Correo: {correo}, Contraseña: {contrasena}");

                var medico = await _context.Medicos
                    .Include(m => m.Departamento)
                    .FirstOrDefaultAsync(m =>
                        m.Correo == correo &&
                        m.ContrasenaHash == contrasena &&
                        m.EsActivo);

                if (medico == null)
                {
                    Console.WriteLine(" Médico no encontrado o credenciales inválidas");
                    return Unauthorized("Credenciales inválidas o cuenta inactiva.");
                }

                Console.WriteLine($" Login exitoso → {medico.Nombre}");

                return Ok(new
                {
                    id_medico = medico.Id_Medico,
                    nombre = medico.Nombre,
                    departamento = medico.Departamento?.Nombre
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error en el login: {ex.Message}");
            }
        }

    }
}
