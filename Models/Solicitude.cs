namespace FinancieraAPI.Models;

public partial class Solicitude
{
    public int SolicitudId { get; set; }

    public int ClienteId { get; set; }

    public DateOnly FechaSolicitud { get; set; }

    public decimal CantidadSolicitada { get; set; }

    public string DestinoPrestamo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public DateOnly? FechaAnalisis { get; set; }

    public int? UserId { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    public virtual Usuario? User { get; set; }
}
