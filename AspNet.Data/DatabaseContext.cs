using AspNet.Data.Models.Entities;
using AspNet.Data.Models.Entities.Map;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Data
{
    public class DatabaseContext : DbContext
    {

        public DbSet<Paciente> Paciente { get; set; } = null!;
		
        public DbSet<Triagem> Triagem { get; set; } = null!;

        public DbSet<Atendimento> Atendimento { get; set; } = null!;

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);

            PacienteMap.Map(modelBuilder.Entity<Paciente>());
            TriagemMap.Map(modelBuilder.Entity<Triagem>());
            AtendimentoMap.Map(modelBuilder.Entity<Atendimento>());

        }
	}
}
