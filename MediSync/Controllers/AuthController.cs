using MediSync.Data;
using MediSync.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            // Buscar paciente por correo
            var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.Correo == email);

            // Si no existe, lo crea automáticamente
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

            // Guardar nombre en sesión
            HttpContext.Session.SetString("PacienteNombre", paciente.Nombre);
            HttpContext.Session.SetInt32("PacienteId", paciente.Id_Paciente);

            return Redirect("/Expediente/Crear");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }
    }
}
