public class Prestamo
{
    public int PrestamoId { get; set; }

    public int NroSocio { get; set; }
    public Socio Socio { get; set; } = null!;

    public string ISBN { get; set; } = string.Empty;
    public Libro Libro { get; set; } = null!;

    public DateTime FechaPrestamo { get; set; }
    public DateTime FechaVencimiento { get; set; }
    public DateTime? FechaDevolucion { get; set; }

    public int EstadoPrestamoId { get; set; }
    public EstadoPrestamo EstadoPrestamo { get; set; } = null!;

    public double MontoMulta { get; set; }

    // Parte adicional (grupo de 3): un préstamo solo puede renovarse una vez.
    public bool Renovado { get; set; }
}
