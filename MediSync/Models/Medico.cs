using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediSync.Models
{
    [Table("Medico")]
    public class Medico
    {
        [Key]
        public int Id_Medico { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(100)")]
        public string? Especialidad { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? Correo { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string? ContrasenaHash { get; set; }

        public bool EsActivo { get; set; } = true;

        public int? Id_Departamento { get; set; }

        [ForeignKey("Id_Departamento")]
        public Departamento? Departamento { get; set; }
    }
}
