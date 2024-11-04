using AspNet.Core.DTOs;
using AspNet.Core.Models.ResultModel;
using AspNet.Data.Models.Entities;
using AutoMapper;

namespace AspNet.Core.Configurations
{
    public class MapperInitilizer : Profile
	{
		public MapperInitilizer()
		{

			CreateMap<Paciente, PacienteDTO>().ReverseMap();
			CreateMap<Paciente, PacienteResult>().ReverseMap();

            CreateMap<Triagem, TriagemDTO>().ReverseMap();
            CreateMap<Triagem, TriagemResult>().ReverseMap();

            CreateMap<Atendimento, AtendimentoDTO>().ReverseMap();
            CreateMap<Atendimento, AtendimentoResult>().ReverseMap();

        }
	}
}
