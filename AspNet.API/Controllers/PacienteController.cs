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
	public class PacienteController : ControllerBase
	{
		private readonly ILogger<PacienteController> _logger;
		private readonly IMapper _mapper;
		private readonly DatabaseContext _dboContext;

		public PacienteController(ILogger<PacienteController> logger,
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
		public async Task<IActionResult> Create([FromBody] PacienteDTO pacienteDTO)
		{

			_logger.LogInformation($"Tentativa de registro para {pacienteDTO.Email} ");

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{

				var paciente = _mapper.Map<Paciente>(pacienteDTO);
				await _dboContext.Paciente.AddAsync(paciente);
                var pacienteResult = _mapper.Map<PacienteResult>(paciente);

                if (pacienteResult == null)
				{
					return BadRequest();
				}

				await _dboContext.SaveChangesAsync();
				return Created(Request.Path.Value!, pacienteResult);

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
        public async Task<IActionResult> Update([FromBody] PacienteDTO pacienteDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var paciente = _mapper.Map<Paciente>(pacienteDTO);

                var result = _dboContext.Paciente.Update(paciente);
                var pacienteResult = _mapper.Map<PacienteResult>(paciente);

                await _dboContext.SaveChangesAsync();

                return Ok(pacienteResult.ToString());

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
                var paciente = await _dboContext.Paciente
                    .Where(p => !(id.HasValue) || p.Id == id)
                    .ToListAsync();

                var pacienteResult = _mapper.Map<List<PacienteResult>>(paciente);

                return Ok(pacienteResult.ToJson());
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
        public async Task<IActionResult> Delete([FromBody] PacienteDTO pacienteDTO)
        {
            try
            {
                var paciente = await _dboContext.Paciente.FindAsync(pacienteDTO.Id);
               
                if (paciente == null)
                {
                    return BadRequest();
                }

                _dboContext.Remove(paciente);
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
