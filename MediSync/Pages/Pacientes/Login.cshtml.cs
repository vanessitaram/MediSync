using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediSync.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace MediSync.Pages.Pacientes

{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        public string Telefono { get; set; } = string.Empty;

        public string Mensaje { get; set; } = string.Empty;

        public LoginModel(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPostAsync(string Correo, string Telefono)
        {
            var client = _clientFactory.CreateClient();
            var baseUrl = _configuration["ApiBaseUrl"];

            long? telefono = null;
            if (long.TryParse(Telefono, out long tel))
                telefono = tel;

            var datos = new
            {
                correo = string.IsNullOrWhiteSpace(Correo) ? null : Correo,
                telefono = telefono
            };

            var response = await client.PostAsJsonAsync($"{baseUrl}/paciente/login", datos);

            if (!response.IsSuccessStatusCode)
            {
                Mensaje = "Datos incorrectos o cuenta no encontrada.";
                return Page();
            }

            // Limpiar la sesión anterior 
            HttpContext.Session.Clear();

            // Leer respuesta
            var contenido = await response.Content.ReadFromJsonAsync<JsonElement>();

            // Extraer datos del JSON
            string nombre = contenido.GetProperty("nombre").GetString() ?? "Paciente desconocido";
            int id = contenido.GetProperty("id_paciente").GetInt32();

            // Guardar en sesión
            HttpContext.Session.SetString("NombrePaciente", nombre);
            HttpContext.Session.SetInt32("IdPaciente", id);

            // Confirmación por consola
            Console.WriteLine($"[LOGIN DEBUG] Sesión actualizada: {nombre} (ID: {id})");

            return RedirectToPage("/Expediente/Crear");
        }

    }
}
