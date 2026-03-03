using AutoMapper;
using Rafiq.Domain.Entities;
using Rafiq.Application.Dtos.SessionDtos;

namespace Rafiq.Application.MappingProfıles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Session, SessionDto>()
                .ForMember(dest => dest.SpecialistName, opt => opt.MapFrom(src => src.Specialist.FirstName));

            CreateMap<SessionCreateDto, Session>()
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.MediaUrl));
        }
    }
}