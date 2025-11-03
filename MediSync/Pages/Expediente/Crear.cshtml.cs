using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediSync.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MediSync.Pages.Expediente
{
    public class CrearModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public MediSync.Models.Expediente NuevoExpediente { get; set; } = new();

        public string Mensaje { get; set; } = string.Empty;
        public string NombrePaciente { get; set; } = string.Empty;

        public CrearModel(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public void OnGet()
        {
            NombrePaciente = HttpContext.Session.GetString("NombrePaciente") ?? "Paciente desconocido";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var idPaciente = HttpContext.Session.GetInt32("IdPaciente");

                if (idPaciente == null)
                {
                    Mensaje = "No se encontró la sesión del paciente. Inicie sesión nuevamente.";
                    return RedirectToPage("/Pacientes/Login");
                }

                NuevoExpediente.Id_Paciente = idPaciente.Value;

                var client = _clientFactory.CreateClient();
                var baseUrl = _configuration["ApiBaseUrl"];

                var json = JsonSerializer.Serialize(NuevoExpediente);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{baseUrl}/Expediente/registrar", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();

                    using var doc = JsonDocument.Parse(responseBody);
                    string prediagnostico = doc.RootElement.GetProperty("prediagnostico").GetString() ?? "";

                    return RedirectToPage("/Expediente/Confirmacion", new
                    {
                        mensaje = "Expediente registrado correctamente.",
                        prediagnostico = prediagnostico
                    });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al registrar el expediente: {error}";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                Mensaje = $"Error inesperado: {ex.Message}";
                return Page();
            }
        }
    }
}
