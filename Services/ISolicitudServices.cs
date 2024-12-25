using FinancieraAPI.DTOs;

namespace FinancieraAPI.Services
{
    public interface ISolicitudServices
    {
        Task<int> PostSolicitud(SolicitudRequest solicitud);

        Task<List<SolicitudResponse>> GetSolicitudes();

        Task<SolicitudResponse> GetSolicitud(int solicitudId);

        Task<int> PutSolicitud(int SolicitudId, SolicitudRequest solicitud);

        Task<int> DeleteSolicitud(int solicitudId);
    }
}
