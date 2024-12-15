using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Services
{
    public class EmpleoServices : IEmpleoServices
    {
        private readonly FinancieraDbContext _context;
        private readonly IMapper _IMapper;

        public EmpleoServices(FinancieraDbContext context, IMapper iMapper)
        {
            _context = context;
            _IMapper = iMapper;
        }

        public async Task<int> DeleteEmpleo(int empleoId)
        {
            var empleo = await _context.Empleos.FindAsync(empleoId);

            if (empleo == null)
            {
                return -1; // Indica que el empleo no fue encontrado
            }

            _context.Empleos.Remove(empleo);

            return await _context.SaveChangesAsync();
        }

        public async Task<EmpleoResponse> GetEmpleo(int empleoId)
        {
            var empleo = await _context.Empleos.FindAsync(empleoId);

            var empleoResponse = _IMapper.Map<EmpleoResponse>(empleo);

            return empleoResponse;
        }

        public async Task<List<EmpleoResponse>> GetEmpleos()
        {
            var empleos = await _context.Empleos.ToListAsync();

            var empleosList = _IMapper.Map<List<Empleo>, List<EmpleoResponse>>(empleos);

            return empleosList;
        }

        public async Task<int> PostEmpleo(EmpleoRequest empleo)
        {
            var empleoRequest = _IMapper.Map<EmpleoRequest, Empleo>(empleo);

            await _context.Empleos.AddAsync(empleoRequest);

            await _context.SaveChangesAsync();

            return empleoRequest.EmpleoId;
        }

        public async Task<int> PutEmpleo(int empleoId, EmpleoRequest empleo)
        {
            var entity = await _context.Empleos.FindAsync(empleoId);

            if (entity == null)
            {
                return -1; // Indica que el empleo no fue encontrado
            }

            // Actualiza los atributos especificados
            entity.ClienteId = empleo.ClienteID;
            entity.LugarTrabajo = empleo.LugarTrabajo;
            entity.Cargo = empleo.Cargo;
            entity.SueldoBase = empleo.SueldoBase;
            entity.FechaIngreso = empleo.FechaIngreso;
            entity.TelefonoTrabajo = empleo.TelefonoTrabajo;
            entity.DireccionTrabajo = empleo.DireccionTrabajo;

            _context.Empleos.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}
