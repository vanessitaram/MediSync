using MediSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using System.Text.Json;

namespace MediSync.Pages.Pacientes
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        [BindProperty] public string Correo { get; set; } = string.Empty;
        [BindProperty] public string Telefono { get; set; } = string.Empty;
        [BindProperty] public string Contrasena { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public LoginModel(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var baseUrl = _configuration["ApiBaseUrl"];

                long? telefono = null;
                if (long.TryParse(Telefono, out long tel))
                    telefono = tel;

                var datos = new
                {
                    correo = string.IsNullOrWhiteSpace(Correo) ? null : Correo,
                    telefono = telefono,
                    contrasena = Contrasena
                };

                var response = await client.PostAsJsonAsync($"{baseUrl}/api/paciente/login", datos);

                if (!response.IsSuccessStatusCode)
                {
                    Mensaje = "Datos incorrectos o cuenta no encontrada.";
                    return Page();
                }

                var contenido = await response.Content.ReadFromJsonAsync<JsonElement>();
                string nombre = contenido.GetProperty("nombre").GetString() ?? "Paciente";
                int id = contenido.GetProperty("id_paciente").GetInt32();

                // 🔹 Guardar sesión
                HttpContext.Session.Clear();
                HttpContext.Session.SetString("NombrePaciente", nombre);
                HttpContext.Session.SetInt32("IdPaciente", id);
                await HttpContext.Session.CommitAsync();

                Console.WriteLine($"[SESION OK] ID={HttpContext.Session.Id}, Paciente={nombre} ({id})");

                return RedirectToPage("/Expediente/Crear");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en login: {ex.Message}");
                Mensaje = "Error interno al iniciar sesión.";
                return Page();
            }
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Pacientes/Login");
        }
    }
}
