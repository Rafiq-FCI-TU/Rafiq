using AutoMapper;
using Rafiq.Application.Dtos;
using Rafiq.Application.Dtos.SessionDtos;
using Rafiq.Application.Hubs;
using Rafiq.Domain.Entities;

namespace Rafiq.Application.MappingProfiles
{
    public class MappingProfiles : Profile
    {
        private static readonly TimeZoneInfo EgyptTz =
            OperatingSystem.IsWindows()
                ? TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time")
                : TimeZoneInfo.FindSystemTimeZoneById("Africa/Cairo");

        public MappingProfiles()
        {
            #region Chat Mappings

            CreateMap<ChatMessage, ChatMessageDto>()
    .ForCtorParam(nameof(ChatMessageDto.Id),
        opt => opt.MapFrom(src => src.Id))

    .ForCtorParam(nameof(ChatMessageDto.SenderId),
        opt => opt.MapFrom(src => src.SenderId))

    .ForCtorParam(nameof(ChatMessageDto.SenderName),
        opt => opt.MapFrom(src =>
            src.Sender != null
                ? $"{src.Sender.FirstName} {src.Sender.LastName}".Trim()
                : string.Empty))

    .ForCtorParam(nameof(ChatMessageDto.ReceiverId),
        opt => opt.MapFrom(src => src.ReceiverId))

    .ForCtorParam(nameof(ChatMessageDto.ReceiverName),
        opt => opt.MapFrom(src =>
            src.Receiver != null
                ? $"{src.Receiver.FirstName} {src.Receiver.LastName}".Trim()
                : string.Empty))

    .ForCtorParam(nameof(ChatMessageDto.Content),
        opt => opt.MapFrom(src => src.Content))

    .ForCtorParam(nameof(ChatMessageDto.SentAt),
        opt => opt.MapFrom(src =>
            TimeZoneInfo.ConvertTimeFromUtc(src.SentAt, EgyptTz)))

    .ForCtorParam(nameof(ChatMessageDto.IsRead),
        opt => opt.MapFrom(src => src.IsRead))

    .ForCtorParam(nameof(ChatMessageDto.ReadAt),
        opt => opt.MapFrom(src =>
            src.ReadAt.HasValue
                ? TimeZoneInfo.ConvertTimeFromUtc(src.ReadAt.Value, EgyptTz)
                : (DateTime?)null));

            #endregion

            #region Session Mappings

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

            // Session => SessionsBySpecialistDto
            CreateMap<Session, SessionsBySpecialistDto>()
                .ForMember(dest => dest.SpecialistName,
                           opt => opt.MapFrom(src => src.Specialist.FirstName))
                .ForMember(dest => dest.NumberOfSessions,
                           opt => opt.MapFrom(src => src.Specialist.Sessions.Count));

            // PatientSessionAttempt => AttemptDto
            CreateMap<PatientSessionAttempt, AttemptDto>()
                .ForMember(dest => dest.SessionTitle, opt => opt.MapFrom(src => src.Session.Title))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            #endregion

            #region Specialist Mappings

            // SpecialistProfile => SpecialistResponseDto
            CreateMap<SpecialistProfile, SpecialistResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email ?? string.Empty))
                .ForMember(dest => dest.PatientsCount, opt => opt.MapFrom(src => src.SpecialistPatients.Count));

            // SpecialistPatient => SpecialistPatientDto
            CreateMap<SpecialistPatient, SpecialistPatientDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.Patient.FirstName} {src.Patient.LastName}"))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Patient.DateOfBirth))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Patient.Gender.ToString()))
                .ForMember(dest => dest.TotalScore, opt => opt.MapFrom(src => src.Patient.TotalScore))
                .ForMember(dest => dest.FamilyEmail, opt => opt.MapFrom(src => src.Patient.FamilyProfile != null
                    ? src.Patient.FamilyProfile.Email
                    : string.Empty))
                .ForMember(dest => dest.FamilyName, opt => opt.MapFrom(src => src.Patient.FamilyProfile != null
                    ? $"{src.Patient.FamilyProfile.FirstName} {src.Patient.FamilyProfile.LastName}".Trim()
                    : string.Empty))
                .ForMember(dest => dest.FamilyId, opt => opt.MapFrom(src => src.Patient.FamilyProfile != null
                    ? src.Patient.FamilyProfileId
                    : string.Empty));

            #endregion
        }
    }
}