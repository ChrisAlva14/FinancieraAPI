using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FinancieraAPI.Services
{
    public class ClienteServices : IClienteServices
    {
        private readonly FinancieraDbContext _context;
        private readonly IMapper _IMapper;

        public ClienteServices(FinancieraDbContext context, IMapper iMapper)
        {
            _context = context;
            _IMapper = iMapper;
        }

        public async Task<int> DeleteCliente(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);

            if (cliente == null)
            {
                return -1;
            }

            _context.Clientes.Remove(cliente);

            return await _context.SaveChangesAsync();
        }

        public async Task<ClienteResponse> GetCliente(int clienteId)
        {
            var cliente = await _context.Clientes.FindAsync(clienteId);

            var clienteResponse = _IMapper.Map<ClienteResponse>(cliente);

            return clienteResponse;
        }

        public async Task<List<ClienteResponse>> GetClientes()
        {
            var clientes = await _context.Clientes.ToListAsync();

            var clientesList = _IMapper.Map<List<Cliente>, List<ClienteResponse>>(clientes);

            return clientesList;
        }

        public async Task<int> PostCliente(ClienteRequest cliente)
        {
            var clienteRequest = _IMapper.Map<ClienteRequest, Cliente>(cliente);

            await _context.Clientes.AddAsync(clienteRequest);

            await _context.SaveChangesAsync();

            return clienteRequest.ClienteId;
        }

        public async Task<int> PutCliente(int ClienteId, ClienteRequest cliente)
        {
            var entity = await _context.Clientes.FindAsync(ClienteId);

            if (entity == null)
            {
                return -1;
            }

            entity.Nombre = cliente.Nombre;
            entity.FechaNacimiento = cliente.FechaNacimiento;
            entity.Dui = cliente.DUI;
            entity.Nit = cliente.NIT;
            entity.Direccion = cliente.Direccion;
            entity.TelefonoCelular = cliente.TelefonoCelular;
            entity.TelefonoFijo = cliente.TelefonoFijo;
            entity.UserId = cliente.UserID;
            entity.Estado = cliente.Estado;

            _context.Clientes.Update(entity);

            return await _context.SaveChangesAsync();
        }
    }
}
