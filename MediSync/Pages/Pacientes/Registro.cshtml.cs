using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MediSync.Models;
using System.Net.Http.Json;

namespace MediSync.Pages.PacientesPages
{
    public class RegistroModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public string Mensaje { get; set; } = string.Empty;

        public RegistroModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IActionResult> OnPostAsync(string Nombre, int Edad, string Sexo, int Telefono, string Correo)
        {
            var cliente = _clientFactory.CreateClient();
            var paciente = new MediSync.Models.Paciente
            {
                Nombre = Nombre,
                Edad = Edad,
                Sexo = Sexo,
                Telefono = Telefono,
                Correo = Correo
            };

            var resp = await cliente.PostAsJsonAsync("http://localhost:5289/api/paciente/registrar", paciente);

            if (resp.IsSuccessStatusCode)
            {
                Mensaje = "Registro completado correctamente.";
            }
            else
            {
                var error = await resp.Content.ReadAsStringAsync();
                Mensaje = $"Ocurrió un error al registrar al paciente: {error}";
            }


            return Page();
        }
    }
}
