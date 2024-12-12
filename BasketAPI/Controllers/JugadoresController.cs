using AutoMapper;
using AutoMapper.QueryableExtensions;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using BasketAPI.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BasketAPI.Controllers
{
    [Route("api/jugadores")]
    [ApiController]
    public class JugadoresController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "jugadores";
        private readonly string contenedor = "jugadores";

        public JugadoresController(ApplicationDbContext context, IMapper mapper,
                IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
            :base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<JugadorDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<Jugador, JugadorDTO>(paginacion, ordenarPor: j => j.Nombre);
        }



        [HttpGet("{id:int}", Name = "ObtenerJugadorPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<JugadorDTO>> Get(int id)
        {
            return await Get<Jugador, JugadorDTO>(id);
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<EquipoJugadorDTO>>> Get(string nombre)
        {
            return await context.Jugadores.Where(j=>j.Nombre.Contains(nombre))
                .ProjectTo<EquipoJugadorDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }
            



        [HttpPost]
        public async Task<IActionResult> Post([FromForm] JugadorCreacionDTO jugadorCreacionDTO)
        {
            var jugador = mapper.Map<Jugador>(jugadorCreacionDTO);

            if (jugadorCreacionDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, jugadorCreacionDTO.Foto);
                jugador.Foto = url;
            }

            context.Add(jugador);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerJugadorPorId", new { id = jugador.Id }, jugador);

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] JugadorCreacionDTO jugadorCreacionDTO)
        {
            var jugador = await context.Jugadores.FirstOrDefaultAsync(j => j.Id == id);
            if (jugador is null)
            {
                return NotFound();
            }

            jugador = mapper.Map(jugadorCreacionDTO, jugador);

            if (jugadorCreacionDTO.Foto is not null)
            {
                jugador.Foto = await almacenadorArchivos.Editar(jugador.Foto, contenedor,
                    jugadorCreacionDTO.Foto);

            }

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Jugador>(id);
        }
    }
}
