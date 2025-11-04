using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediSync.Pages.Expediente
{
    public class ConfirmacionModel : PageModel
    {
        public string Mensaje { get; set; } = string.Empty;
        public string Prediagnostico { get; set; } = string.Empty;

        public void OnGet(string mensaje, string prediagnostico)
        {
            Mensaje = mensaje ?? "Expediente registrado correctamente.";
            Prediagnostico = prediagnostico ?? string.Empty;
        }
    }
}
