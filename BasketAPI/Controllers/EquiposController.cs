using AutoMapper;
using AutoMapper.QueryableExtensions;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using BasketAPI.Servicios;
using BasketAPI.Utilidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace BasketAPI.Controllers
{
    [Route("api/equipos")]
    [ApiController]
    public class EquiposController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "equipos";
        private readonly string contenedor = "equipos";

        public EquiposController(ApplicationDbContext context, IMapper mapper,
            IOutputCacheStore outputCacheStore,
            IAlmacenadorArchivos almacenadorArchivos)
            : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<EquipoDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<Equipo, EquipoDTO>(paginacion, ordenarPor: e => e.Nombre);
        }


        [HttpGet("categoria/{categoriaId:int}", Name = "ObtenerEquiposPorCategoria")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<EquipoDTO>> GetEquiposPorCategoria(int categoriaId, [FromQuery] PaginacionDTO paginacion)
        {
            //return await Get<Equipo, EquipoDTO>(paginacion, ordenarPor: e => e.Nombre);
            var query = context.Equipos
                   .Where(e => e.CategoriaId == categoriaId) // Filtro directo
                   .OrderBy(e => e.Nombre);                 // Ordenar por nombre

            // Aplica la paginación y proyecta a DTO
            return await query
                .ProjectTo<EquipoDTO>(mapper.ConfigurationProvider)
                .Paginar(paginacion) // Método de extensión para la paginación
                .ToListAsync();
        }

        [HttpGet("landing")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<ListadoEquiposDTO>> Get()
        {
            var top = 12;
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var equiposHistorico = await context.Equipos
                .Where(p => p.FechaFin < hoy)
                .OrderBy(p => p.Nombre)
                .Take(top)
                .ProjectTo<EquipoDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var equiposTemporada = await context.Equipos
                .Where(p => p.FechaFin>= hoy)
                .OrderBy(p => p.Nombre)
                .Take(top)
                .ProjectTo<EquipoDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var resultado = new ListadoEquiposDTO();
            resultado.EquiposTemporada = equiposTemporada;
            resultado.EquiposHistorico = equiposHistorico;
            return resultado;
        }



        [HttpGet("{id:int}", Name = "ObtenerEquipoPorId")]
        [OutputCache(Tags = [cacheTag])]    
        public async Task<ActionResult<EquipoDetallesDTO>> Get(int id)
        {
            var equipo = await context.Equipos
                .ProjectTo<EquipoDetallesDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (equipo is null)
            {
                return NotFound();
            }
            return equipo;
        }

        [HttpGet("filtrar")]
        public async Task<ActionResult<List<EquipoDTO>>> Filtrar([FromQuery] EquiposFiltrarDTO equiposFiltrarDTO)
        {
            var equiposQueryable = context.Equipos.AsQueryable();
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            if (!string.IsNullOrWhiteSpace(equiposFiltrarDTO.Nombre))
            {
                equiposQueryable = equiposQueryable.Where(p => p.Nombre.Contains(equiposFiltrarDTO.Nombre));
            }

            if (equiposFiltrarDTO.EquipoTemporada)
            {
                equiposQueryable = equiposQueryable.Where(p => p.FechaFin >= hoy);
            }

            if (equiposFiltrarDTO.EquipoHistorico)
            {
                    equiposQueryable = equiposQueryable.Where(p => p.FechaFin <= hoy);
            }

            //if (equiposFiltrarDTO.CategoriaId != 0)
            //{
            //    equiposQueryable = equiposQueryable
            //        .Where(p => p.CategoriaId.EquiposCategorias.Select(pg => pg.GeneroId).Contains(peliculasFiltrarDTO.GeneroId));
            //}

            await HttpContext.InsertarParametrosPaginacionEnCabecera(equiposQueryable);

            var equipos = await equiposQueryable.Paginar(equiposFiltrarDTO.Paginacion)
                .ProjectTo<EquipoDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return equipos;

        }


        [HttpPost]
        public async Task<IActionResult> Post([FromForm] EquipoCreacionDTO equipoCreacionDTO)
        {
            var equipo = mapper.Map<Equipo>(equipoCreacionDTO);

            if (equipoCreacionDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, equipoCreacionDTO.Foto);
                equipo.Foto = url;
            }
            AsignarOrdenEntrenadores(equipo);
            AsignarOrdenJugadores(equipo);
            context.Add(equipo);    // aqui es donde graba realmente sino se hace este paso el registro no se graba
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var equipoDTO = mapper.Map<EquipoDTO>(equipo);
            return CreatedAtRoute("ObtenerEquipoPorId", new { id = equipo.Id }, equipoDTO);

        }



        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<EquiposPutGetDTO>> PutGet(int id)
        {
            var equipo = await context.Equipos
                                .ProjectTo<EquipoDetallesDTO>(mapper.ConfigurationProvider)
                                .FirstOrDefaultAsync(x => x.Id == id);

            if (equipo is null)
            {
                return NotFound();
            }


            var respuesta = new EquiposPutGetDTO();
            respuesta.Equipo = equipo;
            respuesta.Jugadores = equipo.Jugadores;
            respuesta.Entrenadores = equipo.Entrenadores;
            return respuesta;
        }



        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] EquipoCreacionDTO equipoCreacionDTO)
        {
            var equipo = await context.Equipos
                            .Include(p => p.EquiposJugadores)
                            .Include(p => p.EquiposEntrenadores)
                            .FirstOrDefaultAsync(p => p.Id == id);

            if (equipo is null)
            {
                return NotFound();
            }

            equipo = mapper.Map(equipoCreacionDTO, equipo);

            if (equipoCreacionDTO.Foto is not null)
            {
                equipo.Foto = await almacenadorArchivos.Editar(equipo.Foto,
                    contenedor, equipoCreacionDTO.Foto);
            }

            AsignarOrdenEntrenadores(equipo);
            AsignarOrdenJugadores(equipo);

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();
        }

        private void AsignarOrdenJugadores(Equipo equipo)
        {
            if (equipo.EquiposJugadores is not null)
            {
                for (byte i = 0; i < equipo.EquiposJugadores.Count; i++) {
                    equipo.EquiposJugadores[i].Orden = i;
                }
            }
        }

        private void AsignarOrdenEntrenadores(Equipo equipo)
        {
            if (equipo.EquiposEntrenadores is not null)
            {
                for (byte i = 0; i < equipo.EquiposEntrenadores.Count; i++)
                {
                    equipo.EquiposEntrenadores[i].Orden = i;
                }
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Equipo>(id);
        }
    }

}
