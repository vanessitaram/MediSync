using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediSync.Models
{
    [Table("Expediente")]
    public class Expediente
    {
        [Key]
        public int Id_Expediente { get; set; }

        [Required]
        public int Id_Paciente { get; set; }

        [Required]
        public DateTime Fecha_Ingreso { get; set; }

        [Required]
        public TimeSpan Hora_Ingreso { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Sintomas { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(30)")]
        public string Estado { get; set; } = "En proceso";

        public int? Id_Medico { get; set; }

        public int? Id_Departamento { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? Prediagnostico { get; set; }

        [ForeignKey("Id_Paciente")]
        public Paciente? Paciente { get; set; }

        [ForeignKey("Id_Medico")]
        public Medico? Medico { get; set; }

        [ForeignKey("Id_Departamento")]
        public Departamento? Departamento { get; set; }
    }
}
