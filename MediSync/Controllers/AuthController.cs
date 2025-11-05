using MediSync.Data;
using MediSync.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MediSync.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly MediSyncContext _context;

        public AuthController(MediSyncContext context)
        {
            _context = context;
        }


        [HttpGet("login-google")]
        public IActionResult LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleResponse")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded) return Redirect("/Pacientes/Login");

            var email = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Email)?.Value;
            var name = result.Principal.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value;

            if (email == null)
                return Redirect("/Pacientes/Login");

            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Correo == email);

            if (paciente == null)
            {
                paciente = new Paciente
                {
                    Nombre = name ?? "Sin nombre",
                    Correo = email,
                    Fecha_Registro = DateTime.Now
                };
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();
            }

            HttpContext.Session.SetString("PacienteNombre", paciente.Nombre);
            HttpContext.Session.SetInt32("PacienteId", paciente.Id_Paciente);

            return Redirect("/Expediente/Crear");
        }

        // -------------------- LOGIN DE MÉDICOS (MANUAL) --------------------

        [HttpPost("login-medico")]
        public async Task<IActionResult> LoginMedico([FromForm] string correo, [FromForm] string contrasena)
        {
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasena))
                return BadRequest("Debe ingresar correo y contraseña.");

            var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Correo == correo && m.EsActivo);
            if (medico == null)
                return Unauthorized("No existe un médico registrado con este correo.");

            string contrasenaHash = HashPassword(contrasena);

            if (medico.ContrasenaHash != contrasenaHash)
                return Unauthorized("Contraseña incorrecta.");

            HttpContext.Session.SetString("MedicoNombre", medico.Nombre);
            HttpContext.Session.SetInt32("MedicoId", medico.Id_Medico);
            HttpContext.Session.SetInt32("DepartamentoId", medico.Id_Departamento ?? 0);

            return Redirect("/Medico/Panel");
        }

        // -------------------- CAMBIO DE CONTRASEÑA --------------------

        [HttpPost("cambiar-contrasena")]
        public async Task<IActionResult> CambiarContrasena([FromForm] int idMedico, [FromForm] string nuevaContrasena)
        {
            var medico = await _context.Medicos.FindAsync(idMedico);
            if (medico == null) return NotFound("Médico no encontrado.");

            medico.ContrasenaHash = HashPassword(nuevaContrasena);
            await _context.SaveChangesAsync();

            return Ok("Contraseña actualizada correctamente.");
        }


        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Redirect("/");
        }


        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
    }
}
