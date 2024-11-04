using System.ComponentModel.DataAnnotations;

namespace AspNet.Core.DTOs
{
    public class PacienteDTO
    {

        public int? Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        [DataType(DataType.PhoneNumber)]
        public string Telefone { get; set; } = string.Empty;

        public string Sexo { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

    }
}
