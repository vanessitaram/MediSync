using MediSync.Data;
using MediSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;

namespace MediSync.Pages.PacientesPages
{
    public class RegistroModel : PageModel
    {
        private readonly MediSyncContext _context;

        public RegistroModel(MediSyncContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Mensaje { get; set; } = string.Empty;

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(
            string Nombre, int Edad, string Sexo, long Telefono, string? Correo,
            string Contrasena, string? NombreContacto, long? TelefonoContacto, string? ParentescoContacto)
        {
            if (string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(Contrasena))
            {
                Mensaje = "Por favor completa todos los campos obligatorios.";
                return Page();
            }

            var existente = _context.Pacientes.FirstOrDefault(p => p.Telefono == Telefono || p.Correo == Correo);
            if (existente != null)
            {
                Mensaje = "Ya existe un paciente registrado con ese teléfono o correo.";
                return Page();
            }

            var paciente = new Paciente
            {
                Nombre = Nombre,
                Edad = Edad,
                Sexo = Sexo,
                Telefono = Telefono,
                Correo = Correo,
                Contrasena = HashPassword(Contrasena),
                NombreContacto = NombreContacto,
                TelefonoContacto = TelefonoContacto,
                ParentescoContacto = ParentescoContacto,
                Fecha_Registro = DateTime.Now
            };

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            Mensaje = "Registro exitoso. Tu información fue guardada correctamente.";
            return Page();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
