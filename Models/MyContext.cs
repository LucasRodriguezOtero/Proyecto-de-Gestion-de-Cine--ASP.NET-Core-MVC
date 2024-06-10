using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TP_FINAL_GRUPO_C.Models;

namespace TP_FINAL_GRUPO_C.Models
{
    public class MyContext : DbContext
    {

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Funcion> Funciones { get; set; }
        public DbSet<Sala> Salas { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<UsuarioFuncion> UF { get; set; }

        public MyContext() { }

        public MyContext(DbContextOptions<MyContext> optionsBuilder) : base(optionsBuilder) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #region
            //connectionStr = "Data Source=DESKTOP-4S2EH6K\\SQLEXPRESS01;Initial Catalog=cine;Integrated Security=True";
            //Console.WriteLine(Properties.Resources.ConnectionString);
            //optionsBuilder.UseSqlServer(Properties.Resources.ConnectionString);
            //optionsBuilder.EnableSensitiveDataLogging();
            //optionsBuilder.UseSqlServer("Data Source=DESKTOP-LA0HVTF\\SQLEXPRESS;Initial Catalog=cine;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
            //Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;
            //Trusted Connection y Trust Server Certificate se agregaron por un tema de permisos que no nos dejaba escribir en la base de datos
            // MultipleActiveResultSet = Se permite la lectura de mas de una conexion en simultaneo. Al tener esto apagado no se nos permitia actualizar 
            // los intentos fallidos del usuario y luego guardar la actualizacion en la base de datos.
            #endregion

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Context");
            optionsBuilder.UseSqlServer(connectionString);

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //nombre de la tabla
            modelBuilder.Entity<Usuario>()
                .ToTable("Usuarios")
                .HasKey(u => u.ID);
            //Agregamos las tablas nuevas
            modelBuilder.Entity<Funcion>()
                .ToTable("Funciones")
                .HasKey(f => f.ID);
            modelBuilder.Entity<Sala>()
                .ToTable("Salas")
                .HasKey(s => s.ID);
            modelBuilder.Entity<Pelicula>()
                .ToTable("Peliculas")
                .HasKey(p => p.ID);
            modelBuilder.Entity<UsuarioFuncion>()
                .ToTable("UsuarioFuncion")
                .HasKey(uf => uf.ID);



            modelBuilder.Entity<Funcion>()
            .HasOne(f => f.MiSala)
            .WithMany(s => s.MisFunciones)
            .HasForeignKey(f => f.idSala)
            .OnDelete(DeleteBehavior.Cascade);


            //ONE TO MANY FUNCION -> PELICULAS
            modelBuilder.Entity<Funcion>()
            .HasOne(f => f.MiPelicula)
            .WithMany(p => p.MisFunciones)
            .HasForeignKey(f => f.idPelicula)
            .OnDelete(DeleteBehavior.Cascade);

            //DEFINICIÓN DE LA RELACIÓN MANY TO MANY USUARIO <-> FUNCION
            modelBuilder.Entity<Usuario>()
                .HasMany(U => U.MisFunciones)
                .WithMany(f => f.Clientes)
                .UsingEntity<UsuarioFuncion>(
                    euf => euf
                    .HasOne(uf => uf.funcion)
                    .WithMany(f => f.UserFuncion)
                    .HasForeignKey(u => u.idFuncion)
                    .OnDelete(DeleteBehavior.Cascade),
                    euf => euf
                    .HasOne(uf => uf.usuario)
                    .WithMany(u => u.UserFuncion)
                    .HasForeignKey(u => u.idUsuario)
                    .OnDelete(DeleteBehavior.Cascade),
                    euf => euf
                    .HasKey(k => new { k.idUsuario, k.idFuncion })
                );

            //propiedades de los datos
            modelBuilder.Entity<Usuario>(
                usr =>
                {
                    usr.Property(u => u.DNI).HasColumnType("bigint");
                    usr.Property(u => u.Nombre).HasColumnType("varchar(50)");
                    usr.Property(u => u.Apellido).HasColumnType("varchar(50)");
                    usr.Property(u => u.Mail).HasColumnType("varchar(512)");
                    usr.Property(u => u.Password).HasColumnType("varchar(50)");
                    usr.Property(u => u.IntentosFallidos).HasColumnType("int");
                    usr.Property(u => u.Bloqueado).HasColumnType("bit");
                    usr.Property(u => u.Credito).HasColumnType("decimal(18,2)");
                    usr.Property(u => u.FechaNacimiento).HasColumnType("datetime");
                    usr.Property(u => u.EsAdmin).HasColumnType("bit");
                });

            modelBuilder.Entity<Funcion>(
                fun =>
                {
                    fun.Property(f => f.idSala).HasColumnType("integer");
                    fun.Property(f => f.idPelicula).HasColumnType("integer");
                    fun.Property(f => f.Fecha).HasColumnType("datetime");
                    fun.Property(f => f.AsientosDisponibles).HasColumnType("integer");
                    fun.Property(f => f.CantidadClientes).HasColumnType("integer");
                    fun.Property(f => f.Costo).HasColumnType("decimal(18,2)");
                });


            modelBuilder.Entity<Sala>(
               sal =>
               {
                   sal.Property(s => s.Ubicacion).HasColumnType("varchar(50)");
                   sal.Property(s => s.Capacidad).HasColumnType("integer");
               });

            modelBuilder.Entity<Pelicula>(
               pel =>
               {
                   pel.Property(p => p.Nombre).HasColumnType("varchar(50)");
                   pel.Property(p => p.Descripcion).HasColumnType("varchar(150)");
                   pel.Property(p => p.Sinopsis).HasColumnType("varchar(255)");
                   pel.Property(p => p.Poster).HasColumnType("varchar(255)");
                   pel.Property(p => p.Duracion).HasColumnType("integer");
               });

            modelBuilder.Entity<UsuarioFuncion>(
               uf =>
               {
                   uf.Property(s => s.idUsuario).HasColumnType("integer");
                   uf.Property(s => s.idFuncion).HasColumnType("integer");
                   uf.Property(s => s.CantidadEntradasCompradas).HasColumnType("integer");
               });



            //AGREGO ALGUNOS DATOS DE PRUEBA

            modelBuilder.Entity<Sala>().HasData(
              new { ID = 1, Ubicacion = "Olivos", Capacidad = 100 },
              new { ID = 2, Ubicacion = "San Isidro", Capacidad = 200 },
              new { ID = 3, Ubicacion = "Caballito", Capacidad = 500 },
              new { ID = 4, Ubicacion = "Palermo", Capacidad = 300 },
              new { ID = 5, Ubicacion = "Recoleta", Capacidad = 150 },
              new { ID = 6, Ubicacion = "Belgrano", Capacidad = 100 },
              new { ID = 7, Ubicacion = "La Plata", Capacidad = 200 },
              new { ID = 8, Ubicacion = "Mar del Plata", Capacidad = 150 },
              new { ID = 9, Ubicacion = "Córdoba", Capacidad = 120 },
              new { ID = 10, Ubicacion = "Rosario", Capacidad = 200 },
              new { ID = 11, Ubicacion = "Mendoza", Capacidad = 130 },
              new { ID = 12, Ubicacion = "Tigre", Capacidad = 150 },
              new { ID = 13, Ubicacion = "Quilmes", Capacidad = 200 },
              new { ID = 14, Ubicacion = "San Miguel", Capacidad = 170 },
              new { ID = 15, Ubicacion = "Lomas de Zamora", Capacidad = 180 },
              new { ID = 16, Ubicacion = "Morón", Capacidad = 200 },
              new { ID = 17, Ubicacion = "Avellaneda", Capacidad = 120 },
              new { ID = 18, Ubicacion = "Banfield", Capacidad = 110 },
              new { ID = 19, Ubicacion = "E.T. el Extraterrestre", Capacidad = 1 });

            modelBuilder.Entity<Pelicula>().HasData(
             new { ID = 1, Nombre = "Toy Story", Descripcion = "Un juguete", Sinopsis = "Uno", Poster = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Toy_Story_4_logo.svg/1200px-Toy_Story_4_logo.svg.png", Duracion = 2 },
             new { ID = 2, Nombre = "Toy Story 2", Descripcion = "Un juguete 2", Sinopsis = "Dos", Poster = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Toy_Story_4_logo.svg/1200px-Toy_Story_4_logo.svg.png", Duracion = 2 },
             new { ID = 3, Nombre = "Toy Story 3", Descripcion = "Un juguete 3", Sinopsis = "Tres", Poster = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Toy_Story_4_logo.svg/1200px-Toy_Story_4_logo.svg.png", Duracion = 2 },
             new { ID = 4, Nombre = "Toy Story 4", Descripcion = "Basta de la misma pelicula", Sinopsis = "Cuatro", Poster = "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3e/Toy_Story_4_logo.svg/1200px-Toy_Story_4_logo.svg.png", Duracion = 2 });

            modelBuilder.Entity<Funcion>().HasData(
                 new { ID = 1, Fecha = Convert.ToDateTime("1992-10-02 12:00:00.000"), AsientosDisponibles = 200, Costo = 2000.0, idSala = 1, idPelicula = 1, CantidadClientes = 0 },
                 new { ID = 2, Fecha = Convert.ToDateTime("1990-09-02 12:00:00.000"), AsientosDisponibles = 150, Costo = 2500.0, idSala = 1, idPelicula = 2, CantidadClientes = 0 },
                 new { ID = 3, Fecha = Convert.ToDateTime("1995-08-02 12:00:00.000"), AsientosDisponibles = 250, Costo = 1500.0, idSala = 2, idPelicula = 2, CantidadClientes = 0 },
                 new { ID = 4, Fecha = Convert.ToDateTime("1997-11-03 12:00:00.000"), AsientosDisponibles = 175, Costo = 1000.0, idSala = 2, idPelicula = 2, CantidadClientes = 0 },
                 new { ID = 5, Fecha = Convert.ToDateTime("1992-11-02 12:00:00.000"), AsientosDisponibles = 300, Costo = 2200.0, idSala = 2, idPelicula = 3, CantidadClientes = 0 },
                 new { ID = 6, Fecha = Convert.ToDateTime("1992-06-02 12:00:00.000"), AsientosDisponibles = 300, Costo = 2200.0, idSala = 2, idPelicula = 3, CantidadClientes = 0 },
                 new { ID = 7, Fecha = Convert.ToDateTime("1992-11-02 12:00:00.000"), AsientosDisponibles = 200, Costo = 1800.0, idSala = 3, idPelicula = 3, CantidadClientes = 0 },
                 new { ID = 8, Fecha = Convert.ToDateTime("2023-7-23 12:00:00.000"), AsientosDisponibles = 180, Costo = 1500.0, idSala = 3, idPelicula = 4, CantidadClientes = 0 },
                 new { ID = 9, Fecha = Convert.ToDateTime("2023-7-16 12:00:00.000"), AsientosDisponibles = 250, Costo = 1200.0, idSala = 4, idPelicula = 4, CantidadClientes = 0 },
                 new { ID = 10, Fecha = Convert.ToDateTime("1992-11-02 12:00:00.000"), AsientosDisponibles = 175, Costo = 1000.0, idSala = 4, idPelicula = 4, CantidadClientes = 0 },
                 new { ID = 11, Fecha = Convert.ToDateTime("2023-01-03 12:00:00.000"), AsientosDisponibles = 300, Costo = 2000.0, idSala = 1, idPelicula = 4, CantidadClientes = 0 },
                 new { ID = 12, Fecha = Convert.ToDateTime("2023-02-04 12:00:00.000"), AsientosDisponibles = 280, Costo = 2200.0, idSala = 1, idPelicula = 1, CantidadClientes = 0 },
                 new { ID = 13, Fecha = Convert.ToDateTime("2023-03-02 12:00:00.000"), AsientosDisponibles = 220, Costo = 1900.0, idSala = 2, idPelicula = 2, CantidadClientes = 0 },
                 new { ID = 14, Fecha = Convert.ToDateTime("2023-04-02 12:00:00.000"), AsientosDisponibles = 190, Costo = 1700.0, idSala = 2, idPelicula = 3, CantidadClientes = 0 },
                 new { ID = 15, Fecha = Convert.ToDateTime("2023-05-02 12:00:00.000"), AsientosDisponibles = 230, Costo = 1400.0, idSala = 3, idPelicula = 2, CantidadClientes = 0 },
                 new { ID = 16, Fecha = Convert.ToDateTime("2023-06-02 12:00:00.000"), AsientosDisponibles = 200, Costo = 1200.0, idSala = 3, idPelicula = 1, CantidadClientes = 0 },
                 new { ID = 17, Fecha = Convert.ToDateTime("2023-07-02 12:00:00.000"), AsientosDisponibles = 250, Costo = 1000.0, idSala = 4, idPelicula = 2, CantidadClientes = 0 },
                 new { ID = 18, Fecha = Convert.ToDateTime("2023-08-02 12:00:00.000"), AsientosDisponibles = 300, Costo = 2200.0, idSala = 4, idPelicula = 2, CantidadClientes = 0 },
                 new { ID = 19, Fecha = Convert.ToDateTime("2023-09-02 12:00:00.000"), AsientosDisponibles = 270, Costo = 1900.0, idSala = 1, idPelicula = 3, CantidadClientes = 0 },
                 new { ID = 20, Fecha = Convert.ToDateTime("2023-10-02 12:00:00.000"), AsientosDisponibles = 230, Costo = 1700.0, idSala = 1, idPelicula = 4, CantidadClientes = 0 },
                 new { ID = 21, Fecha = Convert.ToDateTime("2023-11-02 12:00:00.000"), AsientosDisponibles = 190, Costo = 1400.0, idSala = 2, idPelicula = 1, CantidadClientes = 0 });


            modelBuilder.Entity<Usuario>().HasData(
                new { ID = 1, DNI = 1111111, Nombre = "Pepe", Apellido = "Perez", Mail = "pepe@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1992-11-02 12:00:00.000"), Credito = 100.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 2, DNI = 2222222, Nombre = "El", Apellido = "Admin", Mail = "admin@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1990-06-08 12:00:00.000"), Credito = 0.0, EsAdmin = true, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 3, DNI = 3333333, Nombre = "Lucas", Apellido = "Rodriguez", Mail = "lucas@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1995-08-24 12:00:00.000"), Credito = 3000.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 4, DNI = 4444444, Nombre = "Ana", Apellido = "González", Mail = "ana@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1988-05-15 12:00:00.000"), Credito = 500.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 5, DNI = 5555555, Nombre = "María", Apellido = "López", Mail = "maria@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1993-09-30 12:00:00.000"), Credito = 200.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 6, DNI = 666666, Nombre = "Luis", Apellido = "Fernández", Mail = "luis@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1994-12-10 12:00:00.000"), Credito = 800.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 7, DNI = 77777777, Nombre = "Laura", Apellido = "Martínez", Mail = "laura@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1991-07-18 12:00:00.000"), Credito = 1500.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 8, DNI = 88888888, Nombre = "Carlos", Apellido = "Gómez", Mail = "carlos@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1996-03-25 12:00:00.000"), Credito = 1200.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 9, DNI = 999999, Nombre = "Ana", Apellido = "Rodríguez", Mail = "ana.rodriguez@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1989-09-12 12:00:00.000"), Credito = 2500.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 10, DNI = 1000000, Nombre = "Marcelo", Apellido = "López", Mail = "marcelo@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1997-11-28 12:00:00.000"), Credito = 300.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 11, DNI = 1112111, Nombre = "Julia", Apellido = "Torres", Mail = "julia@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1993-02-04 12:00:00.000"), Credito = 1800.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 12, DNI = 12321312, Nombre = "Mariano", Apellido = "García", Mail = "mariano@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1995-06-22 12:00:00.000"), Credito = 5000.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 13, DNI = 312312312, Nombre = "Gabriela", Apellido = "López", Mail = "gabriela@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1990-10-08 12:00:00.000"), Credito = 700.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 14, DNI = 3123123, Nombre = "Federico", Apellido = "Pérez", Mail = "federico@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1994-04-16 12:00:00.000"), Credito = 1000.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 15, DNI = 312312, Nombre = "Carolina", Apellido = "Fernández", Mail = "carolina@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1991-08-14 12:00:00.000"), Credito = 250.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 16, DNI = 2141241, Nombre = "Luciana", Apellido = "González", Mail = "luciana@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1988-12-30 12:00:00.000"), Credito = 3500.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 17, DNI = 321312, Nombre = "Martín", Apellido = "Martínez", Mail = "martin@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1997-03-17 12:00:00.000"), Credito = 900.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 18, DNI = 412123, Nombre = "Paula", Apellido = "Gómez", Mail = "paula@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1992-09-24 12:00:00.000"), Credito = 2000.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 19, DNI = 421321, Nombre = "Diego", Apellido = "Rodríguez", Mail = "diego.rodriguez@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1996-01-11 12:00:00.000"), Credito = 3000.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 20, DNI = 421412, Nombre = "Sofía", Apellido = "López", Mail = "sofia@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1993-05-28 12:00:00.000"), Credito = 600.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 },
                new { ID = 21, DNI = 412412, Nombre = "Agustín", Apellido = "Torres", Mail = "agustin@mail.com", Password = "123", FechaNacimiento = Convert.ToDateTime("1989-11-01 12:00:00.000"), Credito = 1400.0, EsAdmin = false, Bloqueado = false, IntentosFallidos = 0 });

            modelBuilder.Entity<UsuarioFuncion>().HasData(
                new { idUsuario = 1, idFuncion = 1, CantidadEntradasCompradas = 1, ID=1 },
                new { idUsuario = 1, idFuncion = 2, CantidadEntradasCompradas = 4, ID = 2 },
                new { idUsuario = 1, idFuncion = 3, CantidadEntradasCompradas = 1, ID = 3 },
                new { idUsuario = 3, idFuncion = 4, CantidadEntradasCompradas = 3, ID = 4 },
                new { idUsuario = 3, idFuncion = 5, CantidadEntradasCompradas = 2, ID = 5 },
                new { idUsuario = 4, idFuncion = 9, CantidadEntradasCompradas = 2, ID = 6 },
                new { idUsuario = 4, idFuncion = 10, CantidadEntradasCompradas = 4, ID = 7 },
                new { idUsuario = 4, idFuncion = 11, CantidadEntradasCompradas = 2, ID = 8 },
                new { idUsuario = 6, idFuncion = 12, CantidadEntradasCompradas = 3, ID = 9 },
                new { idUsuario = 6, idFuncion = 13, CantidadEntradasCompradas = 1, ID = 10 },
                new { idUsuario = 6, idFuncion = 14, CantidadEntradasCompradas = 4, ID = 11 },
                new { idUsuario = 8, idFuncion = 15, CantidadEntradasCompradas = 2, ID = 12 },
                new { idUsuario = 8, idFuncion = 16, CantidadEntradasCompradas = 3, ID = 13 },
                new { idUsuario = 8, idFuncion = 17, CantidadEntradasCompradas = 1, ID = 14 },
                new { idUsuario = 10, idFuncion = 18, CantidadEntradasCompradas = 4, ID = 15 },
                new { idUsuario = 10, idFuncion = 19, CantidadEntradasCompradas = 2, ID = 16 },
                new { idUsuario = 10, idFuncion = 20, CantidadEntradasCompradas = 3, ID = 17 });

        }
    }
}
