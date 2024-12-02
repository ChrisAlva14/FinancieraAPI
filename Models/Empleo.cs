using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Empleo
{
    public int EmpleoId { get; set; }

    public int ClienteId { get; set; }

    public string LugarTrabajo { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public decimal SueldoBase { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public string? TelefonoTrabajo { get; set; }

    public string? DireccionTrabajo { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
}
