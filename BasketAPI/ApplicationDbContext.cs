using BasketAPI.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BasketAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<EquipoEntrenador>().HasKey(e => new { e.EquipoId, e.EntrenadorId });

            modelBuilder.Entity<EquipoJugador>().HasKey(e =>new { e.EquipoId,e.JugadorId});

            modelBuilder.Entity<EntrenamientoEntrenador>().HasKey(e => new { e.EntrenamientoId, e.EntrenadorId});

            modelBuilder.Entity<EntrenamientoJugador>().HasKey(e => new { e.EntrenamientoId, e.JugadorId});

            modelBuilder.Entity<EntrenamientoTarea>().HasKey(e => new { e.EntrenamientoId, e.TareaId});


            // Configuración de Tarea <-> Categoria (N:M)
            modelBuilder.Entity<TareaCategoria>().HasKey(tc => new { tc.TareaId, tc.CategoriaId });

            modelBuilder.Entity<TareaElementoTecnico>().HasKey(te => new { te.TareaId, te.ElementoTecnicoId });


        }

        public DbSet<Categoria> Categorias { get; set; } 
        public DbSet<Entrenador> Entrenadores { get; set; }
        public DbSet<Entrenamiento> Entrenamientos { get; set; }
        public  DbSet<EntrenamientoEntrenador> EntrenamientosEntrenadores { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<EquipoEntrenador> EquiposEntrenadores { get; set; }
        public DbSet<EquipoJugador> EquiposJugadores { get; set; }
        public DbSet<ElementoTecnico> ElementosTecnicos { get; set; }
        public DbSet<Jugador> Jugadores { get; set; }
        public DbSet<Partido> Partidos { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        public DbSet<TareaCategoria> TareasCategorias { get; set; }
        public DbSet<TareaElementoTecnico> TareasElementosTecnicos { get; set; }
        public DbSet<TareaImagen> TareasImagenes { get; set; }



    }
}
