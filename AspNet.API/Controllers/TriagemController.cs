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
    public class TriagemController : ControllerBase
    {
        private readonly ILogger<TriagemController> _logger;
        private readonly IMapper _mapper;
        private readonly DatabaseContext _dboContext;

        public TriagemController(ILogger<TriagemController> logger,
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
        public async Task<IActionResult> Create([FromBody] TriagemDTO TriagemDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var triagem = _mapper.Map<Triagem>(TriagemDTO);
                var result = await _dboContext.Triagem.AddAsync(triagem);

                if (result == null)
                {
                    return BadRequest();
                }

                await _dboContext.SaveChangesAsync();
                return Created(Request.Path.Value!, triagem);

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
        public async Task<IActionResult> Update([FromBody] TriagemDTO TriagemDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var triagem = await _dboContext.Triagem
                    .Where(p => p.Id == TriagemDTO.Id)
                    .Include(a => a.Atendimento)
                    .ToListAsync();

                if (triagem == null)
                {
                    return BadRequest();
                }

                triagem[0].Peso = TriagemDTO.Peso;
                triagem[0].Sintomas = TriagemDTO.Sintomas;
                triagem[0].Altura = TriagemDTO.Altura;
                triagem[0].PressaoArterial = TriagemDTO.PressaoArterial;
                triagem[0].Atendimento.Status = Data.Models.enums.AtendimentoStatus.AguardandoEspecialista;

                await _dboContext.SaveChangesAsync();

                var triagemDto = _mapper.Map<TriagemResult>(triagem);

                return Ok(triagemDto.ToString());

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
        public async Task<IActionResult> Get([FromQuery] int? id = null)
        {
            try
            {
                var triagem = await _dboContext.Triagem
                    .Where(p => !(id.HasValue) || p.Id == id)
                    .Include(p => p.Atendimento)
                    .ToListAsync();

                var triagemResult = _mapper.Map<List<TriagemResult>>(triagem);

                foreach (var item in triagem)
                {
                    item.AtendimentoId = item.Atendimento.Id;
                }

                triagemResult.ForEach((item) =>
                {
                    var atendimento = _dboContext.Atendimento.Where(i => item.AtendimentoId == i.Id)
                                                                .Include(p => p.Paciente)
                                                                .ToList();

                    item.NomePaciente = atendimento.FirstOrDefault()?.Paciente.Nome;
                });

                if (triagemResult == null)
                {
                    return Ok();
                }

                return Ok(triagemResult.ToJson());
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
        public async Task<IActionResult> Delete([FromBody] TriagemDTO triagemDTO)
        {
            try
            {
                var triagem = await _dboContext.Triagem.FindAsync(triagemDTO.Id);

                if (triagem == null)
                {
                    return BadRequest();
                }

                _dboContext.Remove(triagem);
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
