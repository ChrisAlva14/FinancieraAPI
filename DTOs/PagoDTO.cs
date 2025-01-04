﻿namespace FinancieraAPI.DTOs
{
    public class PagoResponse
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public decimal MontoAPagar { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal SaldoAcumulado { get; set; }

        public string Estado { get; set; }
    }

    public class PagoRequest
    {
        public int PagoId { get; set; }

        public int PrestamoId { get; set; }

        public DateOnly FechaPago { get; set; }

        public decimal MontoAPagar { get; set; }

        public decimal MontoPagado { get; set; }

        public decimal SaldoAcumulado { get; set; }

        public string Estado { get; set; }
    }

    public class PagoFuturoResponse
    {
        public int PagoId { get; set; }
        public int PrestamoId { get; set; }
        public DateOnly FechaPago { get; set; }
        public decimal MontoAPagar { get; set; } // Monto a pagar en el futuro
        public decimal SaldoAcumulado { get; set; } // Saldo acumulado
        public string Estado { get; set; }
        public decimal MontoPagado { get; set; } // Campo temporal para el formulario
    }
}
