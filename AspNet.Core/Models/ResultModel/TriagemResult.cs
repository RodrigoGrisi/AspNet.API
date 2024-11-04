using AspNet.Data.Models.Entities;
using Newtonsoft.Json;

namespace AspNet.Core.Models.ResultModel
{
    public class TriagemResult
    {

        public int Id { get; set; }

        public int AtendimentoId { get; set; }

        public string? NomePaciente { get; set; }

        public string? Sintomas { get; set; }

        public string? PressaoArterial { get; set; }

        public decimal Peso { get; set; }

        public decimal Altura { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
