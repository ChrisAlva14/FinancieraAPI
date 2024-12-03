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
            CreateMap<Solicitud, SolicitudResponse>();

            // DTO -> Models
            CreateMap<UserRequest, Usuario>();
            CreateMap<ClienteRequest, Cliente>();
            CreateMap<SolicitudRequest, Solicitud>();
        }
    }
}
