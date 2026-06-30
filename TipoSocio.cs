namespace Biblioteca_App
{
    public class TipoSocio
    {
        public int TipoSocioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int MaxLibros { get; set; }
        public int DiasPrestamo { get; set; }
        public double MultaPorDia { get; set; }

        public ICollection<Socio> Socios { get; set; } = new List<Socio>();
    }
}
