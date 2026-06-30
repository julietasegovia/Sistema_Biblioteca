using Microsoft.EntityFrameworkCore;

namespace Biblioteca_App
{
    public class Biblioteca : DbContext
    {
        public DbSet<Libro> Libros { get; set; } = null!;
        public DbSet<Socio> Socios { get; set; } = null!;
        public DbSet<Prestamo> Prestamos { get; set; } = null!;
        public DbSet<Reserva> Reservas { get; set; } = null!;
        public DbSet<TipoSocio> TiposSocio { get; set; } = null!;
        public DbSet<EstadoPrestamo> EstadosPrestamo { get; set; } = null!;
        public DbSet<EstadoReserva> EstadosReserva { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=biblioteca.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Socio>()
                .HasOne(s => s.TipoSocio).WithMany(t => t.Socios)
                .HasForeignKey(s => s.TipoSocioId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Socio).WithMany(s => s.Prestamos)
                .HasForeignKey(p => p.NroSocio).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Libro).WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.ISBN).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.EstadoPrestamo).WithMany(e => e.Prestamos)
                .HasForeignKey(p => p.EstadoPrestamoId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Socio).WithMany(s => s.Reservas)
                .HasForeignKey(r => r.NroSocio).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Libro).WithMany(l => l.Reservas)
                .HasForeignKey(r => r.ISBN).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.EstadoReserva).WithMany(e => e.Reservas)
                .HasForeignKey(r => r.EstadoReservaId).OnDelete(DeleteBehavior.Restrict);

            // ============================================================
            // Datos semilla (seed data)
            // ============================================================

            modelBuilder.Entity<EstadoPrestamo>().HasData(
                new EstadoPrestamo { EstadoPrestamoId = 1, Nombre = "Activo" },
                new EstadoPrestamo { EstadoPrestamoId = 2, Nombre = "Devuelto" },
                new EstadoPrestamo { EstadoPrestamoId = 3, Nombre = "Vencido" }
            );

            modelBuilder.Entity<EstadoReserva>().HasData(
                new EstadoReserva { EstadoReservaId = 1, Nombre = "Pendiente" },
                new EstadoReserva { EstadoReservaId = 2, Nombre = "Cumplida" },
                new EstadoReserva { EstadoReservaId = 3, Nombre = "Cancelada" }
            );

            modelBuilder.Entity<TipoSocio>().HasData(
                new TipoSocio { TipoSocioId = 1, Nombre = "Estandar", MaxLibros = 3, DiasPrestamo = 14, MultaPorDia = 50 },
                new TipoSocio { TipoSocioId = 2, Nombre = "Premium", MaxLibros = 6, DiasPrestamo = 21, MultaPorDia = 30 }
            );

            modelBuilder.Entity<Socio>().HasData(
                new Socio { NroSocio = 1001, Nombre = "Lucia", Apellido = "Fernandez", Email = "lucia@mail.com", Activo = true, TipoSocioId = 1 },
                new Socio { NroSocio = 1002, Nombre = "Martin", Apellido = "Gomez", Email = "martin@mail.com", Activo = true, TipoSocioId = 1 },
                new Socio { NroSocio = 1003, Nombre = "Carolina", Apellido = "Diaz", Email = "carolina@mail.com", Activo = true, TipoSocioId = 1 },
                new Socio { NroSocio = 1004, Nombre = "Diego", Apellido = "Lopez", Email = "diego@mail.com", Activo = true, TipoSocioId = 2 },
                new Socio { NroSocio = 1006, Nombre = "Federico", Apellido = "Ramirez", Email = "federico@mail.com", Activo = false, TipoSocioId = 1 }
            );

            modelBuilder.Entity<Libro>().HasData(
                new Libro { ISBN = "978-0062316097", Titulo = "Sapiens", Autor = "Yuval Noah Harari", Genero = "Ensayo", CantidadCopias = 3 },
                new Libro { ISBN = "978-0345539434", Titulo = "Cosmos", Autor = "Carl Sagan", Genero = "Ciencia", CantidadCopias = 2 },
                new Libro { ISBN = "978-8437604572", Titulo = "Rayuela", Autor = "Julio Cortazar", Genero = "Novela", CantidadCopias = 1 }
            );

            // Préstamo activo y vencido de Rayuela a nombre de Carolina (1003),
            // para que: copiasDisponibles("Rayuela") = 0 y la devolución genere multa.
            modelBuilder.Entity<Prestamo>().HasData(
                new Prestamo
                {
                    PrestamoId = 1,
                    NroSocio = 1003,
                    ISBN = "978-8437604572",
                    FechaPrestamo = new DateTime(2026, 6, 1),
                    FechaVencimiento = new DateTime(2026, 6, 15),
                    FechaDevolucion = null,
                    EstadoPrestamoId = 1,
                    MontoMulta = 0
                }
            );
        }

        // ============================================================
        // Helpers internos
        // ============================================================

        private int idEstadoPrestamo(string nombre) =>
            EstadosPrestamo.First(e => e.Nombre == nombre).EstadoPrestamoId;

        private int idEstadoReserva(string nombre) =>
            EstadosReserva.First(e => e.Nombre == nombre).EstadoReservaId;

        public void actualizarPrestamosVencidos()
        {
            int idActivo = idEstadoPrestamo("Activo");
            int idVencido = idEstadoPrestamo("Vencido");
            var hoy = DateTime.Today;

            var vencidos = Prestamos
                .Where(p => p.EstadoPrestamoId == idActivo && p.FechaDevolucion == null && p.FechaVencimiento < hoy)
                .ToList();

            foreach (var p in vencidos)
                p.EstadoPrestamoId = idVencido;

            if (vencidos.Count > 0)
                SaveChanges();
        }

        public int copiasDisponibles(string isbn)
        {
            var libro = Libros.First(l => l.ISBN == isbn);
            int idActivo = idEstadoPrestamo("Activo");
            int idVencido = idEstadoPrestamo("Vencido");

            int prestados = Prestamos.Count(p =>
                p.ISBN == isbn && (p.EstadoPrestamoId == idActivo || p.EstadoPrestamoId == idVencido));

            return libro.CantidadCopias - prestados;
        }

        public double multasPendientes(int nroSocio)
        {
            // El modelo no tiene columna de "pago"; toda multa registrada se considera pendiente.
            return Prestamos.Where(p => p.NroSocio == nroSocio && p.MontoMulta > 0).Sum(p => p.MontoMulta);
        }

        // ============================================================
        // Búsquedas
        // ============================================================

        public void mostrarLibros()
        {
            Console.WriteLine("Libros disponibles en la biblioteca:");
            foreach (var libro in Libros.ToList())
                Console.WriteLine($" - [{libro.ISBN}] {libro.Titulo} ({libro.Autor}) | Género: {libro.Genero} " +
                                   $"| Disponibles: {copiasDisponibles(libro.ISBN)}/{libro.CantidadCopias}");
        }

        public List<Libro> buscarLibros(string termino)
        {
            termino = termino.ToLower();
            return Libros.Where(l => l.Titulo.ToLower().Contains(termino) || l.Autor.ToLower().Contains(termino)).ToList();
        }

        public Socio? buscarSocio(int nroSocio) =>
            Socios.Include(s => s.TipoSocio).FirstOrDefault(s => s.NroSocio == nroSocio);

        // ============================================================
        // Préstamo
        // ============================================================

        public void prestar(Socio socio, Libro libro)
        {
            if (!socio.Activo)
            {
                Console.WriteLine("El socio está inactivo y no puede realizar préstamos."); // RN-01
                return;
            }

            if (multasPendientes(socio.NroSocio) > 0)
            {
                Console.WriteLine("El socio tiene multas pendientes. Debe abonarlas antes de retirar libros."); // RN-02
                return;
            }

            if (copiasDisponibles(libro.ISBN) <= 0)
            {
                Console.WriteLine("No hay copias disponibles de este libro. Puede reservarlo con reservar()."); // RN-03
                return;
            }

            int idActivo = idEstadoPrestamo("Activo");
            int idVencido = idEstadoPrestamo("Vencido");
            int prestamosActuales = Prestamos.Count(p =>
                p.NroSocio == socio.NroSocio && (p.EstadoPrestamoId == idActivo || p.EstadoPrestamoId == idVencido));

            if (prestamosActuales >= socio.TipoSocio.MaxLibros)
            {
                Console.WriteLine($"El socio alcanzó su límite de {socio.TipoSocio.MaxLibros} libros simultáneos."); // RN-04
                return;
            }

            var hoy = DateTime.Today;
            var prestamo = new Prestamo
            {
                NroSocio = socio.NroSocio,
                ISBN = libro.ISBN,
                FechaPrestamo = hoy,
                FechaVencimiento = hoy.AddDays(socio.TipoSocio.DiasPrestamo), // RN-05
                FechaDevolucion = null,
                EstadoPrestamoId = idActivo,
                MontoMulta = 0
            };

            Prestamos.Add(prestamo);
            SaveChanges();
            Console.WriteLine($"Préstamo registrado. Vence: {prestamo.FechaVencimiento:dd/MM/yyyy}");
        }

        // ============================================================
        // Devolución
        // ============================================================

        public List<Prestamo> prestamosActivosDeSocio(int nroSocio)
        {
            int idActivo = idEstadoPrestamo("Activo");
            int idVencido = idEstadoPrestamo("Vencido");
            return Prestamos.Include(p => p.Libro).Include(p => p.EstadoPrestamo)
                .Where(p => p.NroSocio == nroSocio && (p.EstadoPrestamoId == idActivo || p.EstadoPrestamoId == idVencido))
                .ToList();
        }

        public void devolver(Prestamo prestamo)
        {
            var hoy = DateTime.Today;
            prestamo.FechaDevolucion = hoy;
            prestamo.EstadoPrestamoId = idEstadoPrestamo("Devuelto");

            if (hoy > prestamo.FechaVencimiento) // RN-06
            {
                int diasDemora = (hoy - prestamo.FechaVencimiento).Days;
                var socio = Socios.Include(s => s.TipoSocio).First(s => s.NroSocio == prestamo.NroSocio);
                prestamo.MontoMulta = diasDemora * socio.TipoSocio.MultaPorDia;
            }

            SaveChanges();
            Console.WriteLine("Devolución registrada.");
            if (prestamo.MontoMulta > 0)
                Console.WriteLine($"Se generó una multa por demora de ${prestamo.MontoMulta}.");

            int idPendiente = idEstadoReserva("Pendiente");
            var reservaMasAntigua = Reservas
                .Where(r => r.ISBN == prestamo.ISBN && r.EstadoReservaId == idPendiente)
                .OrderBy(r => r.FechaReserva)
                .FirstOrDefault();

            if (reservaMasAntigua != null) // RN-07
            {
                reservaMasAntigua.EstadoReservaId = idEstadoReserva("Cumplida");
                SaveChanges();

                var socioReserva = Socios.First(s => s.NroSocio == reservaMasAntigua.NroSocio);
                Console.WriteLine($">> AVISO: el libro quedó disponible para la reserva de " +
                                   $"{socioReserva.Nombre} {socioReserva.Apellido} (socio N° {socioReserva.NroSocio}).");
            }
        }

        // ============================================================
        // Reserva
        // ============================================================

        public void reservar(Socio socio, Libro libro)
        {
            if (!socio.Activo)
            {
                Console.WriteLine("El socio está inactivo y no puede realizar reservas."); // RN-01
                return;
            }

            int idPendiente = idEstadoReserva("Pendiente");
            bool yaTieneReserva = Reservas.Any(r =>
                r.NroSocio == socio.NroSocio && r.ISBN == libro.ISBN && r.EstadoReservaId == idPendiente);

            if (yaTieneReserva)
            {
                Console.WriteLine("El socio ya tiene una reserva activa para este libro."); // RN-08
                return;
            }

            var reserva = new Reserva
            {
                NroSocio = socio.NroSocio,
                ISBN = libro.ISBN,
                FechaReserva = DateTime.Today,
                EstadoReservaId = idPendiente
            };

            Reservas.Add(reserva);
            SaveChanges();
            Console.WriteLine($"Reserva registrada el {reserva.FechaReserva:dd/MM/yyyy}.");
        }

        // ============================================================
        // Detalle de socio
        // ============================================================

        public void mostrarDetalleSocio(Socio socio)
        {
            Console.WriteLine($"Socio: {socio.Nombre} {socio.Apellido} | Tipo: {socio.TipoSocio.Nombre} | Activo: {(socio.Activo ? "Sí" : "No")}");

            var activos = prestamosActivosDeSocio(socio.NroSocio);
            Console.WriteLine($"Préstamos activos ({activos.Count}):");
            foreach (var p in activos)
                Console.WriteLine($" - {p.Libro.Titulo} | Vence: {p.FechaVencimiento:dd/MM/yyyy} | Estado: {p.EstadoPrestamo.Nombre}");

            var historial = Prestamos.Include(p => p.Libro)
                .Where(p => p.NroSocio == socio.NroSocio && p.FechaDevolucion != null).ToList();
            Console.WriteLine($"Historial de devoluciones ({historial.Count}):");
            foreach (var p in historial)
                Console.WriteLine($" - {p.Libro.Titulo} | Devuelto: {p.FechaDevolucion:dd/MM/yyyy} | Multa: ${p.MontoMulta}");

            Console.WriteLine($"Multas pendientes: ${multasPendientes(socio.NroSocio)}");
        }

        // ============================================================
        // Reportes
        // ============================================================

        public void reporteLibrosMasPrestados(int top = 5)
        {
            Console.WriteLine($"Top {top} libros más prestados:");
            var lista = Prestamos.GroupBy(p => p.ISBN)
                .Select(g => new { ISBN = g.Key, Cantidad = g.Count() })
                .OrderByDescending(x => x.Cantidad).Take(top).ToList();

            if (lista.Count == 0) { Console.WriteLine("Sin datos."); return; }

            foreach (var x in lista)
            {
                var libro = Libros.First(l => l.ISBN == x.ISBN);
                Console.WriteLine($" - {libro.Titulo} ({libro.Autor}): {x.Cantidad} préstamos");
            }
        }

        public void reporteSociosConMultas()
        {
            Console.WriteLine("Socios con multas pendientes:");
            var lista = Prestamos.Where(p => p.MontoMulta > 0)
                .GroupBy(p => p.NroSocio)
                .Select(g => new { NroSocio = g.Key, Total = g.Sum(p => p.MontoMulta) })
                .ToList();

            if (lista.Count == 0) { Console.WriteLine("Ningún socio tiene multas pendientes."); return; }

            foreach (var x in lista)
            {
                var socio = Socios.First(s => s.NroSocio == x.NroSocio);
                Console.WriteLine($" - {socio.Nombre} {socio.Apellido} (N° {socio.NroSocio}): ${x.Total}");
            }
        }

        public void reportePrestamosVencidos()
        {
            actualizarPrestamosVencidos();
            int idVencido = idEstadoPrestamo("Vencido");
            var lista = Prestamos.Include(p => p.Socio).Include(p => p.Libro)
                .Where(p => p.EstadoPrestamoId == idVencido).ToList();

            Console.WriteLine("Préstamos vencidos:");
            if (lista.Count == 0) { Console.WriteLine("No hay préstamos vencidos."); return; }

            foreach (var p in lista)
                Console.WriteLine($" - {p.Libro.Titulo} | Socio: {p.Socio.Nombre} {p.Socio.Apellido} | Venció: {p.FechaVencimiento:dd/MM/yyyy}");
        }

        public void reporteDisponibilidad(Libro libro)
        {
            int disponibles = copiasDisponibles(libro.ISBN);
            int idPendiente = idEstadoReserva("Pendiente");
            int reservasPendientes = Reservas.Count(r => r.ISBN == libro.ISBN && r.EstadoReservaId == idPendiente);

            Console.WriteLine($"{libro.Titulo}: {disponibles}/{libro.CantidadCopias} copias disponibles.");
            Console.WriteLine($"Reservas pendientes: {reservasPendientes}");
        }

        public void reporteHistorialSocio(Socio socio)
        {
            Console.WriteLine($"Préstamos de {socio.Nombre} {socio.Apellido}:");
            var prestamos = Prestamos.Include(p => p.Libro).Include(p => p.EstadoPrestamo)
                .Where(p => p.NroSocio == socio.NroSocio).ToList();
            foreach (var p in prestamos)
                Console.WriteLine($" - {p.Libro.Titulo} | Estado: {p.EstadoPrestamo.Nombre} | Préstamo: {p.FechaPrestamo:dd/MM/yyyy} | Vencimiento: {p.FechaVencimiento:dd/MM/yyyy}");

            Console.WriteLine($"Reservas de {socio.Nombre} {socio.Apellido}:");
            var reservas = Reservas.Include(r => r.Libro).Include(r => r.EstadoReserva)
                .Where(r => r.NroSocio == socio.NroSocio).ToList();
            foreach (var r in reservas)
                Console.WriteLine($" - {r.Libro.Titulo} | Estado: {r.EstadoReserva.Nombre} | Fecha: {r.FechaReserva:dd/MM/yyyy}");
        }
    }
}