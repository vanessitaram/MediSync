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
        public string Mensaje { get; set; } = string.Empty;
        public bool RegistroExitoso { get; set; }

        public RegistroModel(MediSyncContext context)
        {
            _context = context;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var nombre = Request.Form["Nombre"].ToString();
                var edad = Convert.ToInt32(Request.Form["Edad"]);
                var sexo = Request.Form["Sexo"].ToString();
                var telefono = Convert.ToInt64(Request.Form["Telefono"]);
                var correo = Request.Form["Correo"].ToString();
                var contrasena = Request.Form["Contrasena"].ToString();
                var confirmar = Request.Form["ConfirmarContrasena"].ToString();
                var nombreContacto = Request.Form["NombreContacto"].ToString();
                var telefonoContacto = Request.Form["TelefonoContacto"].ToString();
                var parentesco = Request.Form["ParentescoContacto"].ToString();

                if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(contrasena))
                {
                    Mensaje = "Por favor completa todos los campos obligatorios.";
                    return Page();
                }

                if (contrasena != confirmar)
                {
                    Mensaje = "Las contraseñas no coinciden.";
                    return Page();
                }

                var existente = _context.Pacientes
                    .FirstOrDefault(p => p.Telefono == telefono || p.Correo == correo);

                if (existente != null)
                {
                    Mensaje = "Ya existe un paciente registrado con ese teléfono o correo.";
                    return Page();
                }

                var paciente = new Paciente
                {
                    Nombre = nombre,
                    Edad = edad,
                    Sexo = sexo,
                    Telefono = telefono,
                    Correo = string.IsNullOrWhiteSpace(correo) ? null : correo,
                    Contraseña = HashPassword(contrasena),
                    NombreContacto = string.IsNullOrWhiteSpace(nombreContacto) ? null : nombreContacto,
                    TelefonoContacto = string.IsNullOrWhiteSpace(telefonoContacto) ? null : Convert.ToInt64(telefonoContacto),
                    ParentescoContacto = string.IsNullOrWhiteSpace(parentesco) ? null : parentesco,
                    Fecha_Registro = DateTime.Now
                };

                _context.Pacientes.Add(paciente);
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    RegistroExitoso = true;
                    Mensaje = "Registro exitoso.";
                    Console.WriteLine($"? Paciente guardado: {paciente.Nombre} - ID: {paciente.Id_Paciente}");
                }
                else
                {
                    Mensaje = "Error al guardar en base de datos.";
                    Console.WriteLine("?? SaveChanges no insertó ningún registro.");
                }

                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error al registrar: {ex.Message}");
                Mensaje = "Error interno al registrar paciente.";
                return Page();
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }
    }
}
