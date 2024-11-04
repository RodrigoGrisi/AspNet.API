using Newtonsoft.Json;

namespace AspNet.Core.Models.ResultModel
{
    public class PacienteResult
	{

        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public override string ToString() => JsonConvert.SerializeObject(this);

    }
}
