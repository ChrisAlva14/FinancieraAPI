﻿using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int PrestamoId { get; set; }

    public DateOnly FechaPago { get; set; }

    public decimal MontoAPagar { get; set; }

    public decimal MontoPagado { get; set; }

    public decimal InteresPagado { get; set; }

    public decimal CapitalPagado { get; set; }

    public decimal SaldoRestante { get; set; }

    public string Estado { get; set; }

    public virtual Prestamo Prestamo { get; set; } = null!;
}
