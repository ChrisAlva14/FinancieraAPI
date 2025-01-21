using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Services
{
    public class PagoServices : IPagoServices
    {
        private readonly FinancieraDbContext _context;
        private readonly IMapper _IMapper;

        public PagoServices(FinancieraDbContext context, IMapper iMapper)
        {
            _context = context;
            _IMapper = iMapper;
        }

        public async Task<int> DeletePago(int pagoId)
        {
            var pago = await _context.Pagos.FindAsync(pagoId);

            if (pago == null)
            {
                return -1; // Indica que el pago no fue encontrado
            }

            _context.Pagos.Remove(pago);

            return await _context.SaveChangesAsync();
        }

        public async Task<PagoResponse> GetPago(int pagoId)
        {
            var pago = await _context.Pagos.FindAsync(pagoId);

            var pagoResponse = _IMapper.Map<PagoResponse>(pago);

            return pagoResponse;
        }

        public async Task<List<PagoResponse>> GetPagos()
        {
            var pagos = await _context.Pagos.ToListAsync();

            var pagosList = _IMapper.Map<List<Pago>, List<PagoResponse>>(pagos);

            return pagosList;
        }

        public async Task<List<PagoFuturoResponse>> GetPagosFuturos(int prestamoId)
        {
            var prestamo = await _context
                .Prestamos.Include(p => p.Pagos)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

            if (prestamo == null)
                throw new KeyNotFoundException("Préstamo no encontrado.");

            var montoTotal = prestamo.MontoAprobado;
            var tasaInteresAnual = prestamo.TasaInteres;
            var meses =
                (prestamo.FechaVencimiento.Year - prestamo.FechaInicio.Year) * 12
                + prestamo.FechaVencimiento.Month
                - prestamo.FechaInicio.Month;

            var tasaInteresMensual = (tasaInteresAnual / 12) / 100; // Convertir a tasa mensual

            // Calcular la cuota mensual usando la fórmula de amortización
            var factor = (decimal)Math.Pow(1 + (double)tasaInteresMensual, meses);
            var cuotaMensual = montoTotal * (tasaInteresMensual * factor) / (factor - 1);
            cuotaMensual = Math.Round(cuotaMensual, 2); // Redondear a 2 decimales

            var saldoRestante = montoTotal; // Inicializar el saldo restante con el monto total del préstamo

            var pagosFuturos = new List<PagoFuturoResponse>();

            for (int i = 1; i <= meses; i++)
            {
                var fechaPago = prestamo.FechaInicio.AddMonths(i);

                // Calcular el interés del mes
                decimal interesPagado = saldoRestante * tasaInteresMensual;
                interesPagado = Math.Round(interesPagado, 2);

                // Calcular el capital del mes
                decimal capitalPagado = cuotaMensual - interesPagado;
                capitalPagado = Math.Round(capitalPagado, 2);

                // Asegurarse de que el capital no sea mayor que el saldo restante
                capitalPagado = Math.Min(capitalPagado, saldoRestante);

                // Calcular el saldo restante después del pago
                saldoRestante -= capitalPagado;
                saldoRestante = Math.Round(saldoRestante, 2);

                // Agregar el pago futuro a la lista
                pagosFuturos.Add(
                    new PagoFuturoResponse
                    {
                        PagoId = i,
                        PrestamoId = prestamo.PrestamoId,
                        FechaPago = fechaPago,
                        MontoAPagar = cuotaMensual,
                        InteresPagado = interesPagado,
                        CapitalPagado = capitalPagado,
                        SaldoRestante = saldoRestante,
                        Estado = "Pendiente",
                    }
                );

                // Si el saldo restante es 0, detener la generación de pagos
                if (saldoRestante <= 0)
                    break;
            }

            return pagosFuturos;
        }

        // Implementación del método para obtener pagos vencidos
        public async Task<List<PagoResponse>> GetPagosVencidos()
        {
            // Obtener la fecha actual
            var fechaActual = DateOnly.FromDateTime(DateTime.Today);

            // Filtrar pagos vencidos (fecha de pago menor a la fecha actual y estado "Pendiente")
            var pagosVencidos = await _context.Pagos
                .Where(p => p.FechaPago < fechaActual && p.Estado == "Pendiente")
                .ToListAsync();

            // Mapear a PagoResponse
            var pagosVencidosResponse = _IMapper.Map<List<Pago>, List<PagoResponse>>(pagosVencidos);

            return pagosVencidosResponse;
        }

        public async Task<int> PostPago(PagoRequest pago)
        {
            var pagoEntity = _IMapper.Map<PagoRequest, Pago>(pago);

            var prestamo = await _context
                .Prestamos.Include(p => p.Pagos)
                .FirstOrDefaultAsync(p => p.PrestamoId == pago.PrestamoId);

            if (prestamo == null)
            {
                throw new KeyNotFoundException("Préstamo no encontrado.");
            }

            // Asignar los valores proporcionados al pago
            pagoEntity.InteresPagado = pago.InteresPagado;
            pagoEntity.CapitalPagado = pago.CapitalPagado;
            pagoEntity.SaldoRestante = pago.SaldoRestante;

            // Agregar el pago a la lista de pagos del préstamo
            prestamo.Pagos.Add(pagoEntity);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            return pagoEntity.PagoId;
        }

        public async Task<int> PutPago(int pagoId, PagoRequest pago)
        {
            var entity = await _context.Pagos.FindAsync(pagoId);

            if (entity == null)
            {
                return -1; // Indica que el pago no fue encontrado
            }

            // Actualiza los atributos especificados
            entity.PrestamoId = pago.PrestamoId;
            entity.FechaPago = pago.FechaPago;
            entity.MontoAPagar = pago.MontoAPagar;
            entity.MontoPagado = pago.MontoPagado;
            entity.SaldoRestante = pago.SaldoRestante;
            entity.Estado = pago.Estado;

            _context.Pagos.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}
