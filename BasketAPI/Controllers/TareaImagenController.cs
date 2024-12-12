using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using BasketAPI.Servicios;

namespace BasketAPI.Controllers
{
    [Route("api/tareasimagenes")]
    [ApiController]
    public class TareasImagenesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;

        private readonly string contenedor = "tareasimagenes"; // Carpeta donde se almacenan las imágenes

        public TareasImagenesController(ApplicationDbContext context, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.context = context;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }
        // Consulta todas las imágenes sin filtrar
        [HttpGet]
        public async Task<ActionResult<List<TareaImagen>>> GetTodasLasImagenes()
        {
            var imagenes = await context.TareasImagenes.ToListAsync();

            if (imagenes == null || !imagenes.Any())
            {
                return NotFound("No se encontraron imágenes.");
            }

            return Ok(imagenes);
        }
        // Consulta todas las imágenes de una tarea por TareaId
        [HttpGet("{tareaId:int}")]
        public async Task<ActionResult<List<TareaImagen>>> GetImagenesPorTareaId(int tareaId)
        {
            var imagenes = await context.TareasImagenes
                .Where(ti => ti.TareaId == tareaId)
                .ToListAsync();

            if (imagenes == null || !imagenes.Any())
            {
                return NotFound("No se encontraron imágenes para esta tarea.");
            }

            return Ok(imagenes);
        }

        // Consulta una imagen específica por su Id
        [HttpGet("imagen/{id:int}")]
        public async Task<ActionResult<TareaImagen>> GetPorId(int id)
        {
            var tareaImagen = await context.TareasImagenes.FirstOrDefaultAsync(ti => ti.Id == id);

            if (tareaImagen == null)
            {
                return NotFound("La imagen solicitada no existe.");
            }

            return Ok(tareaImagen);
        }

        // Alta de una imagen para una tarea
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] TareaImagenCreacionDTO tareaImagenCreacionDTO)
        {
            if (tareaImagenCreacionDTO.Foto is null)
            {
                return BadRequest("No se ha proporcionado ninguna imagen.");
            }

            var tareaImagen = new TareaImagen
            {
                TareaId = tareaImagenCreacionDTO.TareaId,
                Nombre = tareaImagenCreacionDTO.Nombre ?? "Sin Nombre", // Usamos un valor por defecto
                Descripcion = tareaImagenCreacionDTO.Descripcion,
                Orden = tareaImagenCreacionDTO.Orden
            };

            // Almacenamos la imagen y obtenemos la URL de la ruta
            var url = await almacenadorArchivos.Almacenar(contenedor, tareaImagenCreacionDTO.Foto);
            tareaImagen.Foto = url;

            context.TareasImagenes.Add(tareaImagen);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetImagenesPorTareaId), new { tareaId = tareaImagen.TareaId }, tareaImagen);
        }

        // Modificación de una imagen (cambiar la foto)
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] TareaImagenCreacionDTO tareaImagenCreacionDTO)
        {
            var tareaImagen = await context.TareasImagenes.FirstOrDefaultAsync(ti => ti.Id == id);

            if (tareaImagen == null)
            {
                return NotFound("La imagen de la tarea no existe.");
            }

            tareaImagen.Nombre = tareaImagenCreacionDTO.Nombre ?? tareaImagen.Nombre;
            tareaImagen.Descripcion = tareaImagenCreacionDTO.Descripcion;
            tareaImagen.Orden = tareaImagenCreacionDTO.Orden;

            // Si se proporciona una nueva imagen, actualizamos la foto
            if (tareaImagenCreacionDTO.Foto != null)
            {
                tareaImagen.Foto = await almacenadorArchivos.Editar(tareaImagen.Foto, contenedor, tareaImagenCreacionDTO.Foto);
            }

            await context.SaveChangesAsync();
            return NoContent();
        }

        // Baja de una imagen (eliminarla)
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var tareaImagen = await context.TareasImagenes.FirstOrDefaultAsync(ti => ti.Id == id);
            if (tareaImagen == null)
            {
                return NotFound("La imagen no existe.");
            }

            // Eliminar la imagen del almacenamiento
            await almacenadorArchivos.Borrar(tareaImagen.Foto, contenedor);
            context.TareasImagenes.Remove(tareaImagen);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
