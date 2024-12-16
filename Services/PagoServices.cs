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

        public async Task<int> PostPago(PagoRequest pago)
        {
            var pagoRequest = _IMapper.Map<PagoRequest, Pago>(pago);

            await _context.Pagos.AddAsync(pagoRequest);

            await _context.SaveChangesAsync();

            return pagoRequest.PagoId;
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
            entity.MontoPagado = pago.MontoPagado;
            entity.SaldoAcumulado = pago.SaldoAcumulado;

            _context.Pagos.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}
