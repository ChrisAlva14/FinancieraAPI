using FinancieraAPI.DTOs;

namespace FinancieraAPI.Services
{
    public interface IPrestamoServices
    {
        Task<int> PostPrestamo(PrestamoRequest prestamo);

        Task<List<PrestamoResponse>> GetPrestamos();

        Task<PrestamoResponse> GetPrestamo(int prestamoId);

        Task<int> PutPrestamo(int PrestamoId, PrestamoRequest prestamo);

        Task<int> DeletePrestamo(int prestamoId);
    }
}
