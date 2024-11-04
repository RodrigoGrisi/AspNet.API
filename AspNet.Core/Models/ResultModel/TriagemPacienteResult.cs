using Newtonsoft.Json;

namespace AspNet.UserIdentity.Boilerplate.Core.Models.ResultModel
{
    public class TriagemPacienteResult : TriagemResult
    {

        public PacienteResult Paciente { get; set; } = null!;

        public AtendimentoResult? Atendimento { get; set; } = null;

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
