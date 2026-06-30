using System.ComponentModel.DataAnnotations;

namespace Biblioteca_App
{
    public class Libro
    {
        [Key]
        public string ISBN { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string Genero { get; set; } = string.Empty;
        public int CantidadCopias { get; set; }

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
