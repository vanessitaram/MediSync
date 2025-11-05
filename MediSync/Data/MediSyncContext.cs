using MediSync.Models;
using Microsoft.EntityFrameworkCore;

namespace MediSync.Data
{
    public class MediSyncContext : DbContext
    {
        public MediSyncContext(DbContextOptions<MediSyncContext> options)
            : base(options)
        {
        }

        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }

        // public DbSet<Notificacion> Notificaciones { get; set; }
        // public DbSet<ChatIA> ChatsIA { get; set; }
        // public DbSet<ChatMensaje> ChatMensajes { get; set; }
    }
}
