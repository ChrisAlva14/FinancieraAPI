using FinancieraAPI.DTOs;

namespace FinancieraAPI.Services
{
    public interface IPagoServices
    {
        Task<int> PostPago(PagoRequest pago);

        Task<List<PagoResponse>> GetPagos();

        Task<PagoResponse> GetPago(int pagoId);

        Task<int> PutPago(int PagoId, PagoRequest pago);

        Task<int> DeletePago(int pagoId);
    }
}
