﻿namespace FinancieraAPI.DTOs
{
    public class PagoResponse
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public string? MontoPagado { get; set; }

        public string? SaldoAcumulado { get; set; }

        public string? Estado { get; set; }
    }

    public class PagoRequest
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public string? MontoPagado { get; set; }

        public string? SaldoAcumulado { get; set; }

        public string? Estado { get; set; }
    }

    public class PagoFuturoResponse
    {
        public int PagoId { get; set; }
        public int PrestamoId { get; set; }
        public DateOnly FechaPago { get; set; }
        public string MontoAPagar { get; set; } // Monto a pagar en el futuro
        public string SaldoAcumulado { get; set; } // Saldo acumulado
        public string Estado { get; set; }
        public decimal MontoPagado { get; set; } // Campo temporal para el formulario
    }
}
