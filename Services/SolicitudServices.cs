using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Services
{
    public class SolicitudServices : ISolicitudServices
    {
        private readonly FinancieraDbContext _context;
        private readonly IMapper _IMapper;

        public SolicitudServices(FinancieraDbContext context, IMapper iMapper)
        {
            _context = context;
            _IMapper = iMapper;
        }

        public async Task<int> DeleteSolicitud(int solicitudId)
        {
            var solicitud = await _context.Solicitudes.FindAsync(solicitudId);

            if (solicitud == null)
            {
                return -1;
            }

            _context.Solicitudes.Remove(solicitud);

            return await _context.SaveChangesAsync();
        }

        public async Task<SolicitudResponse> GetSolicitud(int solicitudId)
        {
            var solicitud = await _context.Solicitudes.FindAsync(solicitudId);

            var solicitudResponse = _IMapper.Map<SolicitudResponse>(solicitud);

            return solicitudResponse;
        }

        public async Task<List<SolicitudResponse>> GetSolicitudes()
        {
            var solicitudes = await _context.Solicitudes.ToListAsync();

            var solicitudesList = _IMapper.Map<List<Solicitud>,
                List<SolicitudResponse>>(solicitudes);

            return solicitudesList;
        }

        public async Task<int> PostSolicitud(SolicitudRequest solicitud)
        {
            var solicitudRequest = _IMapper.Map<SolicitudRequest, Solicitud>(solicitud);

            await _context.Solicitudes.AddAsync(solicitudRequest);

            await _context.SaveChangesAsync();

            return solicitudRequest.SolicitudId;
        }

        public async Task<int> PutSolicitud(int SolicitudId, SolicitudRequest solicitud)
        {
            var entity = await _context.Solicitudes.FindAsync(SolicitudId);

            if (entity == null)
            {
                return -1;
            }

            entity.ClienteId = solicitud.ClienteID;
            entity.FechaSolicitud = solicitud.FechaSolicitud;
            entity.CantidadSolicitada = solicitud.CantidadSolicitada;
            entity.DestinoPrestamo = solicitud.DestinoPrestamo;
            entity.Estado = solicitud.Estado;
            entity.FechaAnalisis = solicitud.FechaAnalisis;
            entity.UserId = solicitud.UserID;

            _context.Solicitudes.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}