using MediSync.Data;
using MediSync.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MediSync.Pages.Medico
{
    public class ExpedientesModel : PageModel
    {
        private readonly MediSyncContext _context;

        public Models.Medico Medico { get; set; } = new();
        public List<Models.Expediente> Expedientes { get; set; } = new();

        public ExpedientesModel(MediSyncContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var idMedico = HttpContext.Session.GetInt32("IdMedico");
            if (idMedico == null)
                return RedirectToPage("/Pacientes/Login");

            Medico = await _context.Medicos
                .Include(m => m.Departamento)
                .FirstOrDefaultAsync(m => m.Id_Medico == idMedico);

            Expedientes = await _context.Expedientes
                .Include(e => e.Paciente)
                .Include(e => e.Departamento)
                .Where(e => e.Id_Medico == idMedico)
                .OrderByDescending(e => e.Fecha_Ingreso)
                .ToListAsync();

            return Page();
        }
    }
}
