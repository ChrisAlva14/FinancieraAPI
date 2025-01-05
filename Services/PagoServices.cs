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

            // Monto total del préstamo con intereses incluidos
            var montoTotal = prestamo.MontoAprobado;
            var tasaInteres = prestamo.TasaInteres;
            var meses =
                (prestamo.FechaVencimiento.Year - prestamo.FechaInicio.Year) * 12
                + prestamo.FechaVencimiento.Month
                - prestamo.FechaInicio.Month;

            // Cálculo del monto total a pagar con intereses
            var tasaInteresMensual = (tasaInteres / 12) / 100;
            var factor = (decimal)Math.Pow(1 + (double)tasaInteresMensual, -meses);
            var montoMensual = montoTotal * (tasaInteresMensual / (1 - factor));
            montoMensual = Math.Ceiling(montoMensual * 100) / 100; // Redondeo al centavo

            var montoTotalConIntereses = montoMensual * meses; // Total considerando intereses

            // Monto ya pagado
            var montoPagadoTotal = prestamo.Pagos.Sum(p => p.MontoPagado);
            var saldoRestante = montoTotalConIntereses - montoPagadoTotal;

            var pagosFuturos = new List<PagoFuturoResponse>();

            for (int i = 1; i <= meses; i++)
            {
                var fechaPago = prestamo.FechaInicio.AddMonths(i);

                // Ajustar el monto a pagar según el saldo restante
                var montoAPagar = Math.Min(saldoRestante, montoMensual);

                pagosFuturos.Add(
                    new PagoFuturoResponse
                    {
                        PagoId = i,
                        PrestamoId = prestamo.PrestamoId,
                        FechaPago = fechaPago,
                        MontoAPagar = Math.Round(montoAPagar, 2),
                        SaldoAcumulado = Math.Round(saldoRestante, 2),
                        Estado = "Pendiente",
                    }
                );

                saldoRestante -= montoAPagar;

                // Si el saldo restante es 0, detenemos la generación de pagos
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

            var montoTotal = prestamo.MontoAprobado;
            var tasaInteres = prestamo.TasaInteres;
            var meses =
                (prestamo.FechaVencimiento.Year - prestamo.FechaInicio.Year) * 12
                + prestamo.FechaVencimiento.Month
                - prestamo.FechaInicio.Month;

            var tasaInteresMensual = (tasaInteres / 12) / 100;
            var factor = (decimal)Math.Pow(1 + (double)tasaInteresMensual, meses);
            var montoTotalConInteres = montoTotal * factor;

            var montoPagadoTotal = prestamo.Pagos.Sum(p => p.MontoPagado);
            var saldoAcumulado = montoTotalConInteres - montoPagadoTotal;

            saldoAcumulado -= pago.MontoPagado;
            pagoEntity.SaldoAcumulado = Math.Round(saldoAcumulado, 2);

            await _context.Pagos.AddAsync(pagoEntity);
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
            entity.SaldoAcumulado = pago.SaldoAcumulado;
            entity.Estado = pago.Estado;

            _context.Pagos.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}
