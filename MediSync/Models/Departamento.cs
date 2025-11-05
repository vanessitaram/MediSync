using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediSync.Models
{
    [Table("Departamento")]
    public class Departamento
    {
        [Key]
        public int Id_Departamento { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Nombre { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(300)")]
        public string? Descripcion { get; set; }
    }
}
