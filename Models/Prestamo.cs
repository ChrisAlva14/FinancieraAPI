using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    public int SolicitudId { get; set; }

    public int ClienteId { get; set; }

    public string? MontoAprobado { get; set; }

    public string? TasaInteres { get; set; }

    public int Plazo { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public string? Estado { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Solicitud Solicitud { get; set; } = null!;
}
