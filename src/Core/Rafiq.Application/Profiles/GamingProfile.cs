using AutoMapper;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Domain.Entities;

namespace Rafiq.Application.Profiles;

public class GamingProfile : Profile
{
    public GamingProfile()
    {
        // GameLevel → GameLevelDto (IsUnlocked is set manually in service)
        CreateMap<GameLevel, GameLevelDto>()
            .ForMember(dest => dest.IsUnlocked, opt => opt.Ignore());

        // Game → GameDto
        CreateMap<Game, GameDto>();

        // GameCategory → GameCategoryDto
        CreateMap<GameCategory, GameCategoryDto>();
    }
}
