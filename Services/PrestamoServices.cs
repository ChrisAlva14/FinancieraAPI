using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Services
{
    public class PrestamoServices : IPrestamoServices
    {
        private readonly FinancieraDbContext _context;
        private readonly IMapper _IMapper;

        public PrestamoServices(FinancieraDbContext context, IMapper iMapper)
        {
            _context = context;
            _IMapper = iMapper;
        }

        public async Task<int> DeletePrestamo(int prestamoId)
        {
            var prestamo = await _context.Prestamos.FindAsync(prestamoId);

            if (prestamo == null)
            {
                return -1; // Indica que el préstamo no fue encontrado
            }

            _context.Prestamos.Remove(prestamo);

            return await _context.SaveChangesAsync();
        }

        public async Task<PrestamoResponse> GetPrestamo(int prestamoId)
        {
            var prestamo = await _context.Prestamos.FindAsync(prestamoId);

            var prestamoResponse = _IMapper.Map<PrestamoResponse>(prestamo);

            return prestamoResponse;
        }

        public async Task<List<PrestamoResponse>> GetPrestamos()
        {
            var prestamos = await _context.Prestamos.ToListAsync();

            var prestamosList = _IMapper.Map<List<Prestamo>, List<PrestamoResponse>>(prestamos);

            return prestamosList;
        }

        public async Task<int> PostPrestamo(PrestamoRequest prestamo)
        {
            var prestamoRequest = _IMapper.Map<PrestamoRequest, Prestamo>(prestamo);

            await _context.Prestamos.AddAsync(prestamoRequest);

            await _context.SaveChangesAsync();

            return prestamoRequest.PrestamoId;
        }

        public async Task<int> PutPrestamo(int prestamoId, PrestamoRequest prestamo)
        {
            var entity = await _context.Prestamos.FindAsync(prestamoId);

            if (entity == null)
            {
                return -1; // Indica que el préstamo no fue encontrado
            }

            // Actualiza los atributos especificados
            entity.SolicitudId = prestamo.SolicitudId;
            entity.MontoAprobado = prestamo.MontoAprobado;
            entity.TasaInteres = prestamo.TasaInteres;
            entity.FechaInicio = prestamo.FechaInicio;
            entity.FechaVencimiento = prestamo.FechaVencimiento;
            entity.Estado = prestamo.Estado;

            _context.Prestamos.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}
