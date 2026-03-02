using AutoMapper;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Domain.Entities;
namespace Rafiq.Application.Profiles
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<Session, SessionDto>()
                .ForMember(dest => dest.SpecialistName,
                           opt => opt.MapFrom(src => src.Specialist.FirstName));
        }
    }
}
