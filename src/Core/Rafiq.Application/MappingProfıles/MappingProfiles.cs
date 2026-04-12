using AutoMapper;
using Rafiq.Application.Dtos;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Domain.Entities;

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


            CreateMap<Session, SessionsBySpecialistDto>()
                .ForMember(dest => dest.SpecialistName,
                           opt => opt.MapFrom(src => src.Specialist.FirstName));


            CreateMap<PatientSessionAttempt, AttemptDto>()
           .ForMember(dest => dest.SessionTitle, opt => opt.MapFrom(src => src.Session.Title))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            // SpecialistProfile → SpecialistResponseDto
            CreateMap<SpecialistProfile, SpecialistResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.PatientsCount, opt => opt.MapFrom(src => src.SpecialistPatients.Count));

            // SpecialistPatient → SpecialistPatientDto
            CreateMap<SpecialistPatient, SpecialistPatientDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Patient.DateOfBirth))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Patient.Gender.ToString()))
                .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.Patient.TotalScore))
                .ForMember(dest => dest.FamilyEmail, opt => opt.MapFrom(src => src.Patient.FamilyProfile != null
                    ? src.Patient.FamilyProfile.Email
                    : string.Empty));


        }
    }
}