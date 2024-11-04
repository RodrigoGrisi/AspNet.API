using AspNet.Data.Models.enums;

namespace AspNet.Data.Models.Entities
{

    public class Atendimento
    {

        public int Id { get; set; }

        public int NumeroSequencial { get; set; }

        public DateTime DataHoraChegada { get; set; }

        public AtendimentoStatus Status { get; set; }

        public Triagem Triagem { get; set; } = null!;

        public int? PacienteId { get; set; }

        public Paciente Paciente { get; set; } = null!;

    }
}
