using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AspNet.Data.Models.Entities.Map
{
    public static class TriagemMap
	{

		public static void Map(this EntityTypeBuilder<Triagem> entityTypeBuilder)
		{

			entityTypeBuilder.ToTable("triagem");

			entityTypeBuilder.HasKey(p => p.Id);
			entityTypeBuilder.Property(p => p.Id).ValueGeneratedOnAdd();

			entityTypeBuilder.HasOne(e => e.Atendimento)
				.WithOne(e => e.Triagem)
				.HasForeignKey<Triagem>()
				.OnDelete(DeleteBehavior.Cascade)
				.IsRequired(true);

        }
	}
}
