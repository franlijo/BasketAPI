using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using BasketAPI.Servicios;
using AutoMapper.QueryableExtensions;
using System.Diagnostics;
using BasketAPI.Utilidades;
using Microsoft.AspNetCore.Authorization;

namespace BasketAPI.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class TareaController : CustomBaseController

    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private const string cacheTag = "tareas";
        private readonly string contenedor = "tareas";

        public TareaController(ApplicationDbContext context, IMapper mapper, 
            IAlmacenadorArchivos almacenadorArchivos,
            IOutputCacheStore outputCacheStore) 
                : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        [HttpGet("historico")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<TareasHistoricoDTO>> Get()
        {
            var top = 12;
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var tVigentes = await context.Tareas
                .Where(t => !t.FechaFin.HasValue || t.FechaFin >= hoy)
                .OrderBy(t => t.Nombre)
                .Take(top)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var tNoVigentes = await context.Tareas
                .Where(t => t.FechaFin < hoy)
                .OrderBy(t => t.Nombre)
                .Take(top)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            var resultado = new TareasHistoricoDTO();
            resultado.TareasVigentes = tVigentes;
            resultado.TareasNoVigentes = tNoVigentes;
            return resultado;

        }

        [HttpGet("{id:int}", Name = "ObtenerTareaPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<TareaDetallesDTO>> Get(int id)
        {
            var tarea = await context.Tareas
                .ProjectTo<TareaDetallesDTO>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tarea is null)
            {
                return NotFound();
            }
            return tarea;

        }


        [HttpGet("filtrar")]
        public async Task<ActionResult<List<TareaDTO>>> Filtrar([FromQuery] TareasFiltrarDTO tareasFiltrarDTO)
        {
            var tareasQueryable = context.Tareas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tareasFiltrarDTO.Nombre))
            {
                tareasQueryable = tareasQueryable.Where(p => p.Nombre.Contains(tareasFiltrarDTO.Nombre));
            }

            //if (tareasFiltrarDTO..EnCines)
            //{
            //    peliculasQueryable = peliculasQueryable.Where(p =>
            //        p.PeliculasCines.Select(pc => pc.PeliculaId).Contains(p.Id));
            //}

            //if (peliculasFiltrarDTO.ProximosEstrenos)
            //{
            //    var hoy = DateTime.Today;
            //    peliculasQueryable = peliculasQueryable.Where(p => p.FechaLanzamiento > hoy);
            //}

            //if (peliculasFiltrarDTO.GeneroId != 0)
            //{
            //    peliculasQueryable = peliculasQueryable
            //        .Where(p => p.PeliculasGeneros.Select(pg => pg.GeneroId).Contains(peliculasFiltrarDTO.GeneroId));
            //}

            await HttpContext.InsertarParametrosPaginacionEnCabecera(tareasQueryable);

            var tareas = await tareasQueryable.Paginar(tareasFiltrarDTO.Paginacion)
                .ProjectTo<TareaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return tareas;

        }


        [HttpGet("PostGet")]
        public async Task<ActionResult<TareasPostGetDTO>> PostGet()
        {
            var categorias = await context.Categorias
                .ProjectTo<CategoriaDTO>(mapper.ConfigurationProvider).ToListAsync();
            var elementosTecnicos = await context.ElementosTecnicos
                .ProjectTo<ElementoTecnicoDTO>(mapper.ConfigurationProvider).ToListAsync();
            return new TareasPostGetDTO
            {
                Categorias = categorias,
                ElementosTecnicos = elementosTecnicos
            };
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TareaCreacionDTO tareaCreacionDTO)
        {
            var tarea = mapper.Map<Tarea>(tareaCreacionDTO);

            if (tareaCreacionDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, tareaCreacionDTO.Foto);
                tarea.Foto = url;
            }

            AsignarOrdenTareaImagenes(tarea);
            context.Add(tarea);
            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            var tareaDTO = mapper.Map<TareaDTO>(tarea);

            return CreatedAtRoute("ObtenerTareaPorId", new { id = tarea.Id }, tareaDTO);

        }



        [HttpGet("PutGet/{id:int}")]
        public async Task<ActionResult<TareasPutGetDTO>> PutGet(int id)
        {
            var tarea = await context.Tareas
                 .ProjectTo<TareaDetallesDTO>(mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync(x => x.Id == id);

            if (tarea == null)
            {
                return NotFound();
            }

            var categoriasSeleccionadasIds = tarea.Categorias.Select(x => x.Id).ToList();

            var categoriasNoSeleccionadasIds = await context.Categorias
                .Where(g => !categoriasSeleccionadasIds.Contains(g.Id))
                .ProjectTo<CategoriaDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
            var tecnicosSeleccionadosIds = tarea.ElementosTecnicos.Select(x => x.Id).ToList();

            var tecnicosNoSeleccionadosIds = await context.ElementosTecnicos
                .Where(e => !tecnicosSeleccionadosIds.Contains(e.Id))
                .ProjectTo<ElementoTecnicoDTO>(mapper.ConfigurationProvider)
                .ToListAsync();
            var respuesta = new TareasPutGetDTO();
            respuesta.Tarea = tarea;
            respuesta.ElementosTecnicosSeleccionados = tarea.ElementosTecnicos;
            respuesta.ElementosTecnicosNoSeleccionados = tecnicosNoSeleccionadosIds;
            respuesta.CategoriasSeleccionadas = tarea.Categorias;
            respuesta.CategoriasNoSeleccionadas = categoriasNoSeleccionadasIds;
            respuesta.TareaImagenes = tarea.TareaImagenes;
            return respuesta;

        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] TareaCreacionDTO tareaCreacionDTO)
        {
            var tarea = await context.Tareas
                .Include(t => t.TareasCategorias)
                .Include(t => t.TareasElementosTecnicos)
                //.Include(t => t.TareasImagenes)
                .FirstOrDefaultAsync(j => j.Id == id);

            if (tarea is null)
            {
                return NotFound();
            }

            tarea = mapper.Map(tareaCreacionDTO, tarea);

            if (tareaCreacionDTO.Foto is not null)
            {
                tarea.Foto = await almacenadorArchivos.Editar(tarea.Foto, contenedor,
                    tareaCreacionDTO.Foto);
            }

            AsignarOrdenTareaImagenes(tarea);

            await context.SaveChangesAsync();
            await outputCacheStore.EvictByTagAsync(cacheTag, default);
            return NoContent();

        }

        private void AsignarOrdenTareaImagenes(Tarea tarea)
        {
            if (tarea.TareasImagenes is not null)
            {
                {
                    for (byte i = 0; i < tarea.TareasImagenes.Count; i++)
                    {
                        tarea.TareasImagenes[i].Orden = i;
                    }
                }
            }
        }



        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<TareaDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<Tarea, TareaDTO>(paginacion, ordenarPor: e => e.Nombre);
        }







       

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Tarea>(id);
        }






    }

}
