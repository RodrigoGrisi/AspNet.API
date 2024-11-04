using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet.Data.Models.Entities.Map
{
	public static class PacienteMap
	{

		public static void Map(this EntityTypeBuilder<Paciente> entityTypeBuilder)
		{

			entityTypeBuilder.ToTable("paciente");

			entityTypeBuilder.HasKey(p => p.Id);
			entityTypeBuilder.Property(p => p.Id).ValueGeneratedOnAdd();

			entityTypeBuilder
				.HasMany(e => e.Atendimentos)
				.WithOne(e => e.Paciente)
				.HasForeignKey(e => e.PacienteId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
	}
}
