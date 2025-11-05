using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediSync.Models
{
    [Table("Consulta")]
    public class Consulta
    {
        [Key]
        public int Id_Consulta { get; set; }

        [ForeignKey("Expediente")]
        public int Id_Expediente { get; set; }

        [ForeignKey("Paciente")]
        public int Id_Paciente { get; set; }

        [ForeignKey("Medico")]
        public int? Id_Medico { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Fecha_Consulta { get; set; } = DateTime.Now;

        [Column(TypeName = "time")]
        public TimeSpan? Hora_Consulta { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Observacion_Medica { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? DiagnosticoIa { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string? Diagnostico_Final { get; set; }

        public Expediente? Expediente { get; set; }
        public Paciente? Paciente { get; set; }
        public Medico? Medico { get; set; }
    }
}
