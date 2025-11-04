
using MediSync.Data;
using MediSync.Models;
using MediSync.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediSync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpedienteController : ControllerBase
    {
        private readonly MediSyncContext _context;
        private readonly IAService _iaService;

        public ExpedienteController(MediSyncContext context, IAService iaService)
        {
            _context = context;
            _iaService = iaService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Expediente expediente)
        {
            if (string.IsNullOrWhiteSpace(expediente.Sintomas))
                return BadRequest("Los síntomas son obligatorios.");

            _context.Expedientes.Add(expediente);
            await _context.SaveChangesAsync();

            string prediagnostico = await _iaService.GenerarPrediagnosticoAsync(expediente.Sintomas);

            return Ok(new { mensaje = "Expediente registrado", prediagnostico });
        }
    }
}
