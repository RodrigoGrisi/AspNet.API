using AspNet.Core.Models.ResultModel;

namespace AspNet.Core.DTOs
{
    public class TriagemDTO
    {

        public int? Id { get; set; }

        public int AtendimentoId { get; set; }

        public string? Sintomas { get; set; }

        public string? PressaoArterial { get; set; }

        public decimal Peso { get; set; }

        public decimal Altura { get; set; }

	}
}
