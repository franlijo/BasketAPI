using AutoMapper;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using BasketAPI.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BasketAPI.Controllers
{
    [Route("api/entrenamiento")]
    [ApiController]

    public class EntrenamientoController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public EntrenamientoController(ApplicationDbContext context, IMapper mapper )
        {
            this.context = context;
            this.mapper = mapper;
        }
        // consulta todos las fechas de entrenamiento
        [HttpGet]
        public async Task<ActionResult<List<Entrenamiento>>> GetTodasLosEntrenamientos()
        {
            var entrenamientos = await context.Entrenamientos.ToListAsync();

            if (entrenamientos == null || !entrenamientos.Any())
            {
                return NotFound("No se encontraron entrenamientos.");
            }

            return Ok(entrenamientos);
        }

        // Consulta todos los entrenamientos de un equipo
        [HttpGet("{equipoId:int}")]
        public async Task<ActionResult<List<Entrenamiento>>> GetEntrenamientoPorEquipoId(int equipoId)
        {
            var entrenamientos = await context.Entrenamientos
                .Where(ti => ti.EquipoId == equipoId)
                .ToListAsync();

            if (entrenamientos == null || !entrenamientos.Any())
            {
                return NotFound("No se encontraron entrenamientos para ese equipo.");
            }

            return Ok(entrenamientos);
        }

        // Consulta un entrenamiento especifico por su id
        [HttpGet("entrenamiento/{id:int}")]
        public async Task<ActionResult<Entrenamiento>> GetPorId(int id)
        {
            var entrenamiento = await context.Entrenamientos.FirstOrDefaultAsync(ti => ti.Id == id);

            if (entrenamiento == null)
            {
                return NotFound("El entrenamiento solicitado no exite");
            }

            return Ok(entrenamiento);
        }

        // Alta de una entrenamiento para un equipo
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] EntrenamientoCreacionDTO entrenamientoCreacionDTO)
        {

            var entrenamiento = new Entrenamiento
            {
                EquipoId = entrenamientoCreacionDTO.EquipoId,
                HoraInicio = entrenamientoCreacionDTO.HoraInicio, 
                HoraFin= entrenamientoCreacionDTO.HoraFin,
                Direccion = entrenamientoCreacionDTO.Direccion,
            };


            context.Entrenamientos.Add(entrenamiento);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEntrenamientoPorEquipoId), new { equipoId = entrenamiento.EquipoId }, entrenamiento);
        }
        
        // Modificación de una imagen (cambiar la foto)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] EntrenamientoCreacionDTO entrenamientoCreacionDTO)
        {
            var entrenamiento = await context.Entrenamientos.FirstOrDefaultAsync(ti => ti.Id == id);

            if (entrenamiento == null)
            {
                return NotFound("El entrenamiento que quieres modificar no existe ");
            }

            entrenamiento.Direccion = entrenamientoCreacionDTO.Direccion;
            entrenamiento.HoraInicio = entrenamientoCreacionDTO.HoraInicio;
            entrenamiento.HoraFin = entrenamientoCreacionDTO.HoraFin;
            entrenamiento.EquipoId = entrenamientoCreacionDTO.EquipoId;

            await context.SaveChangesAsync();
            return NoContent();
        }

        // Baja de la fecha entrenamiento (eliminarla)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entrenamiento = await context.Entrenamientos.FirstOrDefaultAsync(ti => ti.Id == id);
            if (entrenamiento == null)
            {
                return NotFound("El entrenamiento no existe");
            }


            context.Entrenamientos.Remove(entrenamiento);
            await context.SaveChangesAsync();

            return NoContent();
        }





    }

}
