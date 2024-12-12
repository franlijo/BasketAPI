using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using BasketAPI.Servicios;
using AutoMapper.QueryableExtensions;


namespace BasketAPI.Controllers
{
    [Route("api/entrenadores")]
    [ApiController]
    public class EntrenadorController : CustomBaseController
    {
        private readonly IOutputCacheStore outputCacheStore;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "entrenadores";
        private readonly string contenedor = "entrenadores";

        public EntrenadorController(IOutputCacheStore outputCacheStore, 
                ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
            :base(context, mapper, outputCacheStore, cacheTag)
            
        {
            this.outputCacheStore = outputCacheStore;
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet] //api/entrenadores
        [OutputCache(Tags = [cacheTag])]

        public async Task<List<EntrenadorDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<Entrenador, EntrenadorDTO>(paginacion, ordenarPor: e => e.Nombre);
        }

        [HttpGet("todos")]
        [OutputCache(Tags = [cacheTag])]

        public async Task<List<EntrenadorDTO>> Get()
        {
            return await Get<Entrenador, EntrenadorDTO>(ordenarPor: c => c.Nombre);
        }


        [HttpGet("{id:int}", Name = "ObtenerEntrenadorPorId")] 
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<EntrenadorDTO>> Get(int id)
        {
            return await Get<Entrenador, EntrenadorDTO>(id);

        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<EquipoEntrenadorDTO>>> Get(string nombre)
        {
            return await context.Entrenadores.Where(j => j.Nombre.Contains(nombre))
                .ProjectTo<EquipoEntrenadorDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] EntrenadorCreacionDTO entrenadorCreacionDTO)
        {
            var entrenador = mapper.Map<Entrenador>(entrenadorCreacionDTO);

            if (entrenadorCreacionDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, entrenadorCreacionDTO.Foto);
                entrenador.Foto = url;
            }

            context.Add(entrenador);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);

            return CreatedAtRoute("ObtenerEntrenadorPorId", new { id = entrenador.Id }, entrenador);

        }




        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] EntrenadorCreacionDTO entrenadorCreacionDTO)
        {
            var entrenador = await context.Entrenadores.FirstOrDefaultAsync(j => j.Id == id);
            if (entrenador is null)
            {
                return NotFound();
            }

            entrenador = mapper.Map(entrenadorCreacionDTO, entrenador);

            if (entrenadorCreacionDTO.Foto is not null)
            {
                entrenador.Foto = await almacenadorArchivos.Editar(entrenador.Foto, contenedor,
                    entrenadorCreacionDTO.Foto);

            }

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();

        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Entrenador>(id);
        }


    }
}
