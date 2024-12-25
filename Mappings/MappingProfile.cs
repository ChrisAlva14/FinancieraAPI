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
            CreateMap<Empleo, EmpleoResponse>();
            CreateMap<Prestamo, PrestamoResponse>();
            CreateMap<Pago, PagoResponse>();

            // DTO -> Models
            CreateMap<UserRequest, Usuario>();
            CreateMap<ClienteRequest, Cliente>();
            CreateMap<SolicitudRequest, Solicitud>();
            CreateMap<EmpleoRequest, Empleo>();
            CreateMap<PrestamoRequest, Prestamo>();
            CreateMap<PagoRequest, Pago>();
        }
    }
}
