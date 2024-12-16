using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int PrestamoId { get; set; }

    public DateOnly FechaPago { get; set; }

    public string? MontoPagado { get; set; }

    public string? SaldoAcumulado { get; set; }

    public virtual Prestamo Prestamo { get; set; } = null!;
}
