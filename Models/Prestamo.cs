using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Prestamo
{
    public int PrestamoId { get; set; }

    public int SolicitudId { get; set; }

    public decimal MontoAprobado { get; set; }

    public decimal TasaInteres { get; set; }

    public int Plazo { get; set; }

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public string Estado { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Solicitud Solicitud { get; set; } = null!;
}
