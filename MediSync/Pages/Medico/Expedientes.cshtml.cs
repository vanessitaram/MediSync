using MediSync.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace MediSync.Pages.Medico
{
    public class ExpedientesModel : PageModel
    {
        private readonly MediSyncContext _context;

        public MediSync.Models.Medico Medico { get; set; }
        public List<MediSync.Models.Expediente> Expedientes { get; set; } = new();

        public ExpedientesModel(MediSyncContext context)
        {
            _context = context;
        }

        public void OnGet(int idMedico)
        {
            Medico = _context.Medicos.FirstOrDefault(m => m.Id_Medico == idMedico);

            Expedientes = _context.Expedientes
                .Where(e => e.Id_Medico == idMedico)
                .Include(e => e.Paciente)
                .Include(e => e.Departamento)
                .ToList();
        }
    }
}
