using AutoMapper;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using BasketAPI.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasketAPI.Controllers
{
    [Route("api/partido")]
    [ApiController]
    public class PartidoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public PartidoController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<Entrenamiento>>> GetTodasLosPartidos()
        {
            var partidos = await context.Partidos.ToListAsync();

            if (partidos == null || !partidos.Any())
            {
                return NotFound("No se encontraron partidos.");
            }

            return Ok(partidos);
        }

        // Consulta todos los partidos de un equipo
        [HttpGet("{equipoId:int}")]
        public async Task<ActionResult<List<Entrenamiento>>> GetPartidoPorEquipoId(int equipoId)
        {
            var partidos = await context.Partidos
                .Where(ti => ti.EquipoId == equipoId)
                .ToListAsync();

            if (partidos == null || !partidos.Any())
            {
                return NotFound("No se encontraron partidos para ese equipo.");
            }

            return Ok(partidos);
        }

        // Consulta un partido especifico por su id
        [HttpGet("partido/{id:int}")]
        public async Task<ActionResult<Partido>> GetPorId(int id)
        {
            var partido = await context.Partidos.FirstOrDefaultAsync(ti => ti.Id == id);

            if (partido == null)
            {
                return NotFound("El partido solicitado no exite");
            }

            return Ok(partido);
        }

        // Alta de una partido para un equipo
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PartidoCreacionDTO partidoCreacionDTO)
        {

            var partido = new Partido
            {
                EquipoId = partidoCreacionDTO.EquipoId,
                HoraInicio = partidoCreacionDTO.HoraInicio,
                Direccion = partidoCreacionDTO.Direccion,
                Rival = partidoCreacionDTO.Rival,
                Cronica = partidoCreacionDTO.Cronica,
                Resultado= partidoCreacionDTO.Resultado

    };


            context.Partidos.Add(partido);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPartidoPorEquipoId), new { equipoId = partido.EquipoId }, partido);
        }

        // Modificación de una partido
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] PartidoCreacionDTO partidoCreacionDTO)
        {
            var partido = await context.Partidos.FirstOrDefaultAsync(ti => ti.Id == id);

            if (partido == null)
            {
                return NotFound("El partido que quieres modificar no existe ");
            }

            partido.Direccion = partidoCreacionDTO.Direccion;
            partido.HoraInicio = partidoCreacionDTO.HoraInicio;
            partido.EquipoId = partidoCreacionDTO.EquipoId;
            partido.Rival = partidoCreacionDTO.Rival;
            partido.Cronica = partidoCreacionDTO.Cronica;
            partido.Resultado = partidoCreacionDTO.Resultado;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // Baja de la fecha partido (eliminarla)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var partido = await context.Partidos.FirstOrDefaultAsync(ti => ti.Id == id);
            if (partido == null)
            {
                return NotFound("El partido no existe");
            }


            context.Partidos.Remove(partido);
            await context.SaveChangesAsync();

            return NoContent();
        }





    }

}

