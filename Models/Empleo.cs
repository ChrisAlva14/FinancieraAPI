using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Empleo
{
    public int EmpleoId { get; set; }

    public int ClienteId { get; set; }

    public string? LugarTrabajo { get; set; }

    public string? Cargo { get; set; }

    public string? SueldoBase { get; set; }

    public DateOnly FechaIngreso { get; set; }

    public string? TelefonoTrabajo { get; set; }

    public string? DireccionTrabajo { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
}
