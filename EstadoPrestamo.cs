namespace Biblioteca_App
{
    public class EstadoPrestamo
    {
        public int EstadoPrestamoId { get; set; }
        public string Nombre { get; set; } = string.Empty; // "Activo", "Devuelto", "Vencido"

        public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
    }
}
