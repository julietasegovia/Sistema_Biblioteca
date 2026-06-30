namespace Biblioteca_App
{
    public class Reserva
    {
        public int ReservaId { get; set; }

        public int NroSocio { get; set; }
        public Socio Socio { get; set; } = null!;

        public string ISBN { get; set; } = string.Empty;
        public Libro Libro { get; set; } = null!;

        public DateTime FechaReserva { get; set; }

        public int EstadoReservaId { get; set; }
        public EstadoReserva EstadoReserva { get; set; } = null!;
    }
}
