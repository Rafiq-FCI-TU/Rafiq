using AutoMapper;
using Rafiq.Domain.Entities;
using Rafiq.Application.Dtos.SessionDtos;

namespace Rafiq.Application.MappingProfıles
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Session => SessionDto
            CreateMap<Session, SessionDto>()
                .ForMember(dest => dest.SpecialistName,
                           opt => opt.MapFrom(src => src.Specialist.FirstName));

            // SessionCreateDto => Session
            CreateMap<SessionCreateDto, Session>();

            // Session => Sessionfor_Specialist_patientDto
            CreateMap<Session, Sessionfor_Specialist_patientDto>()
                .ForMember(dest => dest.PatientId,
                           opt => opt.MapFrom((src, dest, destMember, context) =>
                           {
                               return context.Items.ContainsKey("PatientId")
                                   ? (int)context.Items["PatientId"]
                                   : 0;
                           }));
        }
    }
}