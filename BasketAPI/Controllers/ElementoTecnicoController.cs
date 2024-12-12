using AutoMapper;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace BasketAPI.Controllers
{
    [Route("api/elementosTecnicos")]
    [ApiController]

    public class ElementoTecnicoController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheTag = "elementosTecnicos";

        public ElementoTecnicoController(ApplicationDbContext context, 
                IMapper mapper, IOutputCacheStore outputCacheStore) 
                : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }
        [HttpGet]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<ElementoTecnicoDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<ElementoTecnico, ElementoTecnicoDTO>(paginacion, ordenarPor: e => e.Nombre);
        }

        [HttpGet("todos")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<List<ElementoTecnicoDTO>> get()
        {
            return await Get<ElementoTecnico, ElementoTecnicoDTO>(ordenarPor: e => e.Nombre);   
        }

        [HttpGet("{id:int}", Name = "ObtenerElementoTecnicoPorId")]
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<ElementoTecnicoDTO>> Get(int id)
        {
            return await Get<ElementoTecnico, ElementoTecnicoDTO>(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ElementoTecnicoCreacionDTO elementoTecnicoCreacionDTO)
        {
            return await Post<ElementoTecnicoCreacionDTO, ElementoTecnico, ElementoTecnicoDTO>(elementoTecnicoCreacionDTO, "ObtenerElementoTecnicoPorId");
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ElementoTecnicoCreacionDTO elementoTecnicoCreacionDTO)
        {
            return await Put<ElementoTecnicoCreacionDTO, ElementoTecnico>(id, elementoTecnicoCreacionDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<ElementoTecnico>(id);
        }


    }

}
