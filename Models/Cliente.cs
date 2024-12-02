using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Cliente
{
    public int ClienteId { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly FechaNacimiento { get; set; }

    public string Dui { get; set; } = null!;

    public string Nit { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public string? TelefonoCelular { get; set; }

    public string? TelefonoFijo { get; set; }

    public int? UserId { get; set; }

    public virtual ICollection<Empleo> Empleos { get; set; } = new List<Empleo>();

    public virtual ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

    public virtual ICollection<Solicitude> Solicitudes { get; set; } = new List<Solicitude>();

    public virtual Usuario? User { get; set; }

    public string? Estado { get; set; }
}
