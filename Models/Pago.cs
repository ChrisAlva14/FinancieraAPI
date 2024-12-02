using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int PrestamoId { get; set; }

    public DateOnly FechaPago { get; set; }

    public decimal MontoPagado { get; set; }

    public decimal SaldoAcumulado { get; set; }

    public virtual Prestamo Prestamo { get; set; } = null!;
}
