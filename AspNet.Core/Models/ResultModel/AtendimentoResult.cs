using AspNet.Data.Models.enums;
using Newtonsoft.Json;

namespace AspNet.Core.Models.ResultModel
{
    public class AtendimentoResult
    {

        public int Id { get; set; }

        public string? PacienteId { get;set; }

        public string? NomePaciente { get;set;}

        public int NumeroSequencial { get; set; }

        public DateTime DataHoraChegada { get; set; }

        public AtendimentoStatus Status { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
