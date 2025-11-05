using MediSync.Data;
using MediSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace MediSync.Pages.Expediente
{
    public class CrearModel : PageModel
    {
        private readonly MediSyncContext _context;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;

        [BindProperty]
        public MediSync.Models.Expediente NuevoExpediente { get; set; } = new();

        public string? NombrePaciente { get; set; }
        public string? Mensaje { get; set; }
        public List<SelectListItem> Departamentos { get; set; } = new();

        public CrearModel(MediSyncContext context, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _context = context;
            _clientFactory = clientFactory;
            _configuration = configuration;
        }

        public void OnGet()
        {
            var idPaciente = HttpContext.Session.GetInt32("IdPaciente");
            NombrePaciente = HttpContext.Session.GetString("NombrePaciente") ?? "Paciente desconocido";

            if (idPaciente == null)
            {
                Mensaje = "Error: No hay paciente activo en sesión.";
                return;
            }

            Departamentos = _context.Departamentos
                .Select(d => new SelectListItem
                {
                    Value = d.Id_Departamento.ToString(),
                    Text = d.Nombre
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idPaciente = HttpContext.Session.GetInt32("IdPaciente");
            if (idPaciente == null)
            {
                Mensaje = "Error: No se encontró sesión del paciente.";
                return Page();
            }

            NuevoExpediente.Id_Paciente = idPaciente.Value;

            try
            {
                var client = _clientFactory.CreateClient();
                var baseUrl = _configuration["ApiBaseUrl"];

                var response = await client.PostAsJsonAsync($"{baseUrl}/api/Expediente/registrar", NuevoExpediente);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Mensaje = $"Error al registrar expediente: {error}";
                    return Page();
                }

                return RedirectToPage("/Expediente/Confirmacion");
            }
            catch (Exception ex)
            {
                Mensaje = $"Error al enviar datos: {ex.Message}";
                return Page();
            }
        }
    }
}
