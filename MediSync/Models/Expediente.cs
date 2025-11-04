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
    }
}
