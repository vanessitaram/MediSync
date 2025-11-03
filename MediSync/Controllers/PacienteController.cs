using MediSync.Data;
using MediSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediSync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly MediSyncContext _context;

   
        public PacienteController(MediSyncContext context)
        {
            _context = context;
        }

        // Endpoint para registrar un nuevo paciente
        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Paciente p)
        {
            // Verifica que el nombre no esté vacío
            if (string.IsNullOrWhiteSpace(p.Nombre))
                return BadRequest("El nombre es obligatorio.");

            // Busca si ya existe un paciente con el mismo teléfono o correo
            var existente = await _context.Pacientes
                .FirstOrDefaultAsync(x => x.Telefono == p.Telefono || x.Correo == p.Correo);

            // Si ya existe, devuelve el mensaje con el ID correspondiente
            if (existente != null)
                return Ok(new { mensaje = "Paciente ya registrado", id = existente.Id_Paciente });

            // Asigna la fecha actual de registro y guarda el nuevo paciente
            p.Fecha_Registro = DateTime.Now;
            _context.Pacientes.Add(p);
            await _context.SaveChangesAsync();

            // Devuelve respuesta indicando que el registro fue exitoso
            return Ok(new { mensaje = "Registro exitoso", id = p.Id_Paciente });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Paciente cred)
        {
            if (cred == null)
                return BadRequest("El cuerpo de la solicitud está vacío.");

            Console.WriteLine($"[LOGIN DEBUG] Correo: {cred.Correo}, Teléfono: {cred.Telefono}");

            var paciente = await _context.Pacientes
                .FirstOrDefaultAsync(x =>
                    (!string.IsNullOrEmpty(cred.Correo) && x.Correo == cred.Correo) ||
                    (cred.Telefono.HasValue && x.Telefono == x.Telefono)
                );

            if (paciente == null)
            {
                Console.WriteLine("[LOGIN DEBUG] No se encontró el paciente.");
                return Unauthorized("No se encontró una cuenta con esos datos.");
            }

            Console.WriteLine($"[LOGIN DEBUG] Paciente encontrado: {paciente.Nombre}");

            return Ok(new
            {
                mensaje = "Acceso correcto",
                id_paciente = paciente.Id_Paciente,
                nombre = paciente.Nombre
            });
        }


    }
}
