using System;
using System.Collections.Generic;

namespace FinancieraAPI.Models;

public partial class Usuario
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string UserPassword { get; set; } = null!;

    public string UserRole { get; set; } = null!;

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual ICollection<Solicitud> Solicitudes { get; set; } = new List<Solicitud>();
}
