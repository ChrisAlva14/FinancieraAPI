using FinancieraAPI.DTOs;

namespace FinancieraAPI.Services
{
    public interface IClienteServices
    {
        Task<int> PostCliente(ClienteRequest cliente);

        Task<List<ClienteResponse>> GetClientes();

        Task<ClienteResponse> GetCliente(int clienteId);

        Task<int> PutCliente(int ClienteId, ClienteRequest cliente);

        Task<int> DeleteCliente(int clienteId);
    }
}
