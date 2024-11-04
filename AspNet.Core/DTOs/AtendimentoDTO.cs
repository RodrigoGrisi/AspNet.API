using AspNet.Data.Models.enums;

namespace AspNet.Core.DTOs
{
    public class AtendimentoDTO
    {

        public int? Id { get; set; }

        public int PacienteId { get; set; }

        public int NumeroSequencial { get; set; }

        public DateTime DataHoraChegada { get; set; }

        public AtendimentoStatus Status { get; set; }

	}
}
