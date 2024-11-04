namespace AspNet.Data.Models.Entities
{
    public class Paciente
	{

		public int Id { get; set; }

		public string Nome { get; set; } = null!;

        public string Telefone { get; set; } = null!;

        public string Sexo { get; set; } = null!;

        public string Email { get; set; } = null!;

        public ICollection<Atendimento> Atendimentos { get; } = new List<Atendimento>();

    }
}
