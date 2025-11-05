using MediSync.Data;
using MediSync.Models;
using MediSync.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediSync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpedienteController : ControllerBase
    {
        private readonly MediSyncContext _context;
        private readonly IAService _iaService;
        private readonly ILogger<ExpedienteController> _logger;

        public ExpedienteController(MediSyncContext context, IAService iaService, ILogger<ExpedienteController> logger)
        {
            _context = context;
            _iaService = iaService;
            _logger = logger;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar([FromBody] Expediente expediente)
        {
            var result = new Dictionary<string, object>();
            try
            {
                result["Paso"] = "Inicio";

                if (string.IsNullOrWhiteSpace(expediente.Sintomas))
                    return BadRequest("Los síntomas son obligatorios.");

                if (expediente.Id_Departamento == null)
                    return BadRequest("Debe seleccionar un área médica.");

                if (expediente.Id_Paciente == 0)
                    return BadRequest("El Id del paciente es obligatorio.");

                result["Paciente"] = expediente.Id_Paciente;
                result["Departamento"] = expediente.Id_Departamento;

                var medicoAsignado = await _context.Medicos
                    .FirstOrDefaultAsync(m => m.Id_Departamento == expediente.Id_Departamento && m.EsActivo);

                if (medicoAsignado == null)
                    return BadRequest("No hay un médico disponible para esta área.");

                result["MedicoAsignado"] = medicoAsignado.Nombre;

                expediente.Id_Medico = medicoAsignado.Id_Medico;
                expediente.Fecha_Ingreso = DateTime.Now.Date;
                expediente.Hora_Ingreso = DateTime.Now.TimeOfDay;
                expediente.Estado = "En proceso";

                _context.Expedientes.Add(expediente);
                await _context.SaveChangesAsync();
                result["ExpedienteGuardado"] = expediente.Id_Expediente;

                string prediagnostico = await _iaService.GenerarPrediagnosticoAsync(expediente.Sintomas);
                expediente.Prediagnostico = prediagnostico;
                await _context.SaveChangesAsync();
                result["PrediagnosticoGenerado"] = prediagnostico?.Substring(0, Math.Min(100, prediagnostico.Length));

                var consulta = new Consulta
                {
                    Id_Expediente = expediente.Id_Expediente,
                    Id_Paciente = expediente.Id_Paciente,
                    Id_Medico = medicoAsignado.Id_Medico,
                    Fecha_Consulta = DateTime.Now,
                    Hora_Consulta = DateTime.Now.TimeOfDay,
                    Observacion_Medica = "Prediagnóstico automático generado por MediSync IA.",
                    DiagnosticoIa = prediagnostico,
                    Diagnostico_Final = null
                };

                _context.Consultas.Add(consulta);
                int cambios = await _context.SaveChangesAsync();
                result["ConsultaInsertada"] = consulta.Id_Consulta;
                result["CambiosEnBD"] = cambios;

                return Ok(result);
            }
            catch (Exception ex)
            {
                result["Error"] = ex.Message;
                return StatusCode(500, result);
            }
        }

    }
}
