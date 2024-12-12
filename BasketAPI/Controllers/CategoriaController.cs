using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using AutoMapper.QueryableExtensions;

namespace BasketAPI.Controllers
{
    [Route("api/categorias")]
    [ApiController]

    public class CategoriaController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IOutputCacheStore outputCacheStore;
        private const string cacheTag = "categorias";
        
        public CategoriaController(ApplicationDbContext context, 
                IMapper mapper, IOutputCacheStore outputCacheStore) 
                : base(context, mapper, outputCacheStore, cacheTag)
        {
            this.context = context;
            this.mapper = mapper;
            this.outputCacheStore = outputCacheStore;
        }
                
        [HttpGet]
        [OutputCache(Tags = [cacheTag])]

        public async Task<List<CategoriaDTO>> Get([FromQuery] PaginacionDTO paginacion)
        {
            return await Get<Categoria, CategoriaDTO>(paginacion, ordenarPor: c => c.Nombre);
        }

        [HttpGet("todas")]
        [OutputCache(Tags = [cacheTag])]

        public async Task<List<CategoriaDTO>> Get()
        {
            return await Get<Categoria, CategoriaDTO>( ordenarPor: c => c.Nombre);
        }



        [HttpGet("{id:int}", Name = "ObtenerCategoriaPorId")] 
        [OutputCache(Tags = [cacheTag])]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            return await Get<Categoria, CategoriaDTO>(id);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            return await Post<CategoriaCreacionDTO, Categoria, CategoriaDTO>(categoriaCreacionDTO, "ObtenerCategoriaPorId");

        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoriaCreacionDTO categoriaCreacionDTO)
        {
            return await Put<CategoriaCreacionDTO, Categoria>(id, categoriaCreacionDTO);

        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Delete<Categoria>(id);
        }




    }


}
