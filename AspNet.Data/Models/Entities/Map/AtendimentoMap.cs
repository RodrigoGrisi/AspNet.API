using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet.Data.Models.Entities.Map
{
	public static class AtendimentoMap
	{

		public static void Map(this EntityTypeBuilder<Atendimento> entityTypeBuilder)
		{

			entityTypeBuilder.ToTable("atendimento");

			entityTypeBuilder.HasKey(p => p.Id);
			entityTypeBuilder.Property(p => p.Id).ValueGeneratedOnAdd();

			entityTypeBuilder.HasOne(e => e.Paciente)
				.WithMany(e => e.Atendimentos)
				.HasForeignKey(p => p.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

			entityTypeBuilder.Property(c => c.NumeroSequencial)
			.HasDefaultValueSql("NEXT VALUE FOR SequenciaAtendimento");

        }
	}
}
