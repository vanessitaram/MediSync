using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediSync.Models
{
    [Table("Paciente")]
    public class Paciente
    {
        [Key]
        public int Id_Paciente { get; set; }


     [Column(TypeName = "nvarchar(100)")]
        public string? Nombre { get; set; } = string.Empty;

        public int? Edad { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public string? Sexo { get; set; } = string.Empty;

        [Column(TypeName = "bigint")]
        public long? Telefono { get; set; }

   

        [Column(TypeName = "nvarchar(100)")]
        public string? Correo { get; set; }


        public DateTime Fecha_Registro { get; set; } = DateTime.Now;
    }
}
