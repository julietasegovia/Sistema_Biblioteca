namespace Biblioteca_App
{
    public class EstadoReserva
    {
        public int EstadoReservaId { get; set; }
        public string Nombre { get; set; } = string.Empty; // "Pendiente", "Cumplida", "Cancelada"

        public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
    }
}
