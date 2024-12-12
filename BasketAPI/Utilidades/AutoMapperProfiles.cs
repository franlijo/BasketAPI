using AutoMapper;
using BasketAPI.DTOs;
using BasketAPI.Entidades;
using Microsoft.AspNetCore.Identity;
using NetTopologySuite.Geometries;

namespace BasketAPI.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles(GeometryFactory geometryFactory)
        {
            ConfigurarMapeoEntrenadores();
            ConfigurarMapeoEntrenamientos();
            ConfigurarMapeoJugadores();
            ConfigurarMapeoCategorias();
            ConfigurarMapeoEquipos();
            ConfigurarMapeoTareas();
            ConfigurarMapeoElementosTecnicos();
            ConfigurarMapeoUsuarios();
            ConfigurarMapeoPartidos();
        }

        private void ConfigurarMapeoPartidos()
        {
            CreateMap<PartidoCreacionDTO, Partido>();
            CreateMap<Partido, PartidoDTO>();   
        }

        private void ConfigurarMapeoEntrenamientos()
        {
            CreateMap<EntrenamientoCreacionDTO, Entrenamiento>();
            CreateMap<Entrenamiento, EntrenamientoDTO>();   
        }

        private void ConfigurarMapeoUsuarios()
        {
            CreateMap<IdentityUser, UsuarioDTO>();
        }

        

        private void ConfigurarMapeoEquipos()
        {
            CreateMap<EquipoCreacionDTO, Equipo>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore())
                .ForMember(e => e.EquiposEntrenadores, dto =>
                dto.MapFrom(e => e.Entrenadores!.Select(entrenador =>
                new EquipoEntrenador { EntrenadorId = entrenador.Id, Rol = entrenador.Rol, 
                    FechaInicio =entrenador.FechaInicio, FechaFin=entrenador.FechaFin })))
               .ForMember(e => e.EquiposJugadores, dto =>
                dto.MapFrom(e => e.Jugadores!.Select(jugador =>
                new EquipoJugador { JugadorId = jugador.Id, Dorsal = jugador.Dorsal , 
                    FechaInicio = jugador.FechaInicio, FechaFin = jugador.FechaFin})));

            CreateMap<Equipo, EquipoDTO>();

            CreateMap<Equipo, EquipoDetallesDTO>()
                .ForMember(j => j.Jugadores, entidad => entidad.MapFrom(j => j.EquiposJugadores.OrderBy(o => o.Orden)))
                .ForMember(j => j.Entrenadores, entidad => entidad.MapFrom(j => j.EquiposEntrenadores.OrderBy(o => o.Orden)));

            CreateMap<EquipoJugador, EquipoJugadorDTO>()
                .ForMember(dto => dto.Id, entidad => entidad.MapFrom(p => p.JugadorId))
                .ForMember(dto => dto.NombreCorto, entidad => entidad.MapFrom(p => p.Jugador.NombreCorto))
                .ForMember(dto => dto.Nombre, entidad => entidad.MapFrom(p => p.Jugador.Nombre))
                .ForMember(dto => dto.Apellidos, entidad => entidad.MapFrom(p => p.Jugador.Apellidos))
                .ForMember(dto => dto.Foto, entidad => entidad.MapFrom(p => p.Jugador.Foto));

            CreateMap<EquipoEntrenador, EquipoEntrenadorDTO>()
                .ForMember(dto => dto.Id, entidad => entidad.MapFrom(e => e.EntrenadorId))
                .ForMember(dto => dto.NombreCorto, entidad => entidad.MapFrom(e => e.Entrenador.NombreCorto))
                .ForMember(dto => dto.Nombre, entidad => entidad.MapFrom(e => e.Entrenador.Nombre))
                .ForMember(dto => dto.Apellidos, entidad => entidad.MapFrom(e => e.Entrenador.Apellidos))
                .ForMember(dto => dto.Foto, entidad => entidad.MapFrom(e => e.Entrenador.Foto));

            CreateMap<Equipo, EquipoDTO>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Nombre))
                .ForMember(dest => dest.CategoriaGenero, opt => opt.MapFrom(src => src.Categoria.Genero));


        }

        private void ConfigurarMapeoCategorias()
        {
            CreateMap<CategoriaCreacionDTO, Categoria>();
            CreateMap<Categoria, CategoriaDTO>();
             
        }

        private void ConfigurarMapeoElementosTecnicos()
        {
            CreateMap<ElementoTecnicoCreacionDTO, ElementoTecnico>();
            CreateMap<ElementoTecnico, ElementoTecnicoDTO>();
        }

        private void ConfigurarMapeoTareas()
        {
            CreateMap<TareaCreacionDTO, Tarea>()
                .ForMember(dest => dest.Foto, opt => opt.Ignore()) // Foto procesada manualmente
                .ForMember(x => x.TareasCategorias, dto =>dto.MapFrom(c => c.CategoriasIds!
                            .Select(id => new TareaCategoria { CategoriaId = id })))
                .ForMember(x => x.TareasElementosTecnicos, dto =>dto.MapFrom(e => e.ElementosTecnicosIds!.
                            Select(id => new TareaElementoTecnico { ElementoTecnicoId = id })));



            CreateMap<Tarea, TareaDTO>();

            CreateMap<Tarea, TareaDetallesDTO>()
           .ForMember(dest => dest.Categorias, opt => opt.MapFrom(src => src.TareasCategorias.Select(tc => tc.Categoria)))
           .ForMember(dest => dest.ElementosTecnicos, opt => opt.MapFrom(src => src.TareasElementosTecnicos.Select(te => te.ElementoTecnico)))
           .ForMember(dest => dest.TareaImagenes, opt => opt.MapFrom(src => src.TareasImagenes));


            //CreateMap<Tarea, TareaDetallesDTO>()
            //    .ForMember(t => t.Categorias, entidad => entidad.MapFrom(t => t.TareasCategorias))
            //    .ForMember(t => t.ElementosTecnicos, entidad => entidad.MapFrom(t => t.TareasElementosTecnicos))
            //    .ForMember(t => t.TareaImagenes, entidad => entidad.MapFrom(t => t.TareasImagenes));


            //CreateMap<Tarea, TareaDTO>()
            //    .ForMember(c => c.Categorias, entidad =>
            //        entidad.MapFrom(c => c.TareasCategorias))
            //    .ForMember(e => e.ElementosTecnicos, entidad =>
            //        entidad.MapFrom(e => e.TareasElementosTecnicos))
            //    .ForMember(f => f.TareaImagenes, entidad =>
            //        entidad.MapFrom(f => f.TareasImagenes));

            CreateMap<TareaCategoria, CategoriaDTO>()
                .ForMember(c => c.Id, tc => tc.MapFrom(p => p.CategoriaId))
                .ForMember(c => c.Nombre, tc => tc.MapFrom(p => p.Categoria.Nombre));

            CreateMap<TareaElementoTecnico, ElementoTecnicoDTO>()
                .ForMember(e => e.Id, et => et.MapFrom(e => e.ElementoTecnicoId))
                .ForMember(e => e.Nombre, tc => tc.MapFrom(e => e.ElementoTecnico.Nombre));

            CreateMap<TareaImagen, TareaImagenDTO>();

                //.ForMember(dest => dest.TareaImagenes, opt => opt.Ignore()); // Imágenes procesadas manualmente

            CreateMap<TareaImagenCreacionDTO, TareaImagen>()
                .ForMember(dest => dest.Foto, opt => opt.Ignore()); // Foto procesada manualmente


        }


        private void ConfigurarMapeoJugadores()
        {
            CreateMap<JugadorCreacionDTO, Jugador>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());

            CreateMap<Jugador, JugadorDTO>();

            CreateMap<Jugador, EquipoJugadorDTO>();
        }

        private void ConfigurarMapeoEntrenadores()
        {
            CreateMap<EntrenadorCreacionDTO, Entrenador>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());

            CreateMap<Entrenador, EntrenadorDTO>();

            CreateMap<Entrenador, EquipoEntrenadorDTO>();
        }
    }
}
