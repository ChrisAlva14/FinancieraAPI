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
            var prestamo = await _context.Prestamos
                .Include(p => p.Pagos)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

            if (prestamo == null)
                throw new KeyNotFoundException("Préstamo no encontrado.");

            var montoTotal = decimal.Parse(prestamo.MontoAprobado);
            var meses = (prestamo.FechaVencimiento.Year - prestamo.FechaInicio.Year) * 12 +
                        prestamo.FechaVencimiento.Month - prestamo.FechaInicio.Month;

            var montoMensual = montoTotal / meses;
            var montoPagadoTotal = prestamo.Pagos.Sum(p => decimal.Parse(p.MontoPagado));
            var saldoAcumulado = montoTotal - montoPagadoTotal;

            var pagosFuturos = new List<PagoFuturoResponse>();

            for (int i = 1; i <= meses; i++)
            {
                var fechaPago = prestamo.FechaInicio.AddMonths(i);
                var montoAPagar = montoMensual;

                if (saldoAcumulado > 0 && i == meses) // Último mes
                {
                    montoAPagar += saldoAcumulado;
                    saldoAcumulado = 0;
                }

                pagosFuturos.Add(new PagoFuturoResponse
                {
                    PagoId = i,
                    PrestamoId = prestamo.PrestamoId,
                    FechaPago = fechaPago,
                    MontoAPagar = montoAPagar.ToString("F2"),
                    SaldoAcumulado = saldoAcumulado.ToString("F2"),
                    Estado = "Pendiente"
                });

                saldoAcumulado -= montoMensual;
            }

            return pagosFuturos;
        }


        public async Task<int> PostPago(PagoRequest pago)
        {
            var pagoEntity = _IMapper.Map<PagoRequest, Pago>(pago);

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
