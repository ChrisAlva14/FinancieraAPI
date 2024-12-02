using AutoMapper;
using FinancieraAPI.DTOs;
using FinancieraAPI.Models;

namespace FinancieraAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Models -> DTO
            CreateMap<Usuario, UserResponse>();
            CreateMap<Cliente, ClienteResponse>();

            // DTO -> Models
            CreateMap<UserRequest, Usuario>();
            CreateMap<ClienteRequest, Cliente>();
        }
    }
}
