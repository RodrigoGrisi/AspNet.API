using AspNet.API.Filters;
using AspNet.Core.DTOs;
using AspNet.Core.Extensions;
using AspNet.Core.Models.ResultModel;
using AspNet.Data;
using AspNet.Data.Models.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNet.Controllers
{

    [TokenFilter]
    [Route("api/v1/[controller]")]
	[ApiController]
	public class AtendimentoController : ControllerBase
	{
		private readonly ILogger<AtendimentoController> _logger;
		private readonly IMapper _mapper;
		private readonly DatabaseContext _dboContext;

		public AtendimentoController(ILogger<AtendimentoController> logger,
			IMapper mapper,
			DatabaseContext dboContext)
		{
			_logger = logger;
			_mapper = mapper;
			_dboContext = dboContext;

		}

		[HttpPost]
		[Route("create")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Create([FromBody] AtendimentoDTO atendimentoDTO)
		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{

				var atendimento = _mapper.Map<Atendimento>(atendimentoDTO);
                atendimento.DataHoraChegada = atendimento.DataHoraChegada.AddHours(-3);
				var result = await _dboContext.Atendimento.AddAsync(atendimento);

				if (result == null)
				{
					return BadRequest();
				}

				await _dboContext.SaveChangesAsync();
				return Created(Request.Path.Value!, atendimento);

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, $"Algo deu errado no {nameof(Create)}");
				return Problem($"Algo deu errado no {nameof(Create)}", statusCode: 500);
			}
		}


        [HttpPatch]
        [Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] AtendimentoDTO atendimentoDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var atendimento = await _dboContext.Atendimento.FindAsync(atendimentoDTO.Id);

                if(atendimento == null)
                {
                    return BadRequest();
                }

                atendimento.Status = atendimentoDTO.Status;
                atendimento.DataHoraChegada = atendimentoDTO.DataHoraChegada.AddHours(-3);

                await _dboContext.SaveChangesAsync();

                var atendimentoResult = _mapper.Map<AtendimentoResult>(atendimento);
                return Ok(atendimentoResult.ToString());

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Algo deu errado no {nameof(Update)}");
                return Problem($"Algo deu errado no {nameof(Update)}", statusCode: 500);
            }
        }


        [HttpGet]
        [Route("get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] int? id)
        {
            try
            {
                var atendimento = await _dboContext.Atendimento
                    .Where(p => !(id.HasValue) || p.Id == id)
                    .Include(t => t.Paciente)
					.ToListAsync();

                var atendimentoResults = _mapper.Map<IEnumerable<AtendimentoResult>>(atendimento);

                foreach(var atendimentoResult in atendimentoResults)
                {

                    var paciente = atendimento.FirstOrDefault(p => p.Id == atendimentoResult.Id)!.Paciente;

                    atendimentoResult.PacienteId = Convert.ToString(paciente.Id);
                    atendimentoResult.NomePaciente = paciente.Nome;
                }

                return Ok(atendimentoResults.ToJson());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Algo deu errado no {nameof(Get)}");
                return Problem($"Algo deu errado no {nameof(Get)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromBody] AtendimentoDTO atendimentoDTO)
        {
            try
            {

                var triagem = await _dboContext.Triagem
                    .Where(t=> t.AtendimentoId == atendimentoDTO.Id)
                    .ToListAsync();

                var atendimento = await _dboContext.Atendimento.
                    FindAsync(atendimentoDTO.Id);

                if (atendimento == null)
                {
                    return BadRequest();
                }

                if(triagem != null)
                {
                    _dboContext.RemoveRange(triagem);
                }

                _dboContext.Remove(atendimento);
                await _dboContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Algo deu errado no {nameof(Delete)}");
                return Problem($"Algo deu errado no {nameof(Delete)}", statusCode: 500);
            }
        }

    }
}
