using FinancieraAPI.DTOs;

namespace FinancieraAPI.Services
{
    public interface IEmpleoServices
    {
        Task<int> PostEmpleo(EmpleoRequest empleo);

        Task<List<EmpleoResponse>> GetEmpleos();

        Task<EmpleoResponse> GetEmpleo(int empleoId);

        Task<int> PutEmpleo(int EmpleoId, EmpleoRequest empleo);

        Task<int> DeleteEmpleo(int empleoId);
    }
}
