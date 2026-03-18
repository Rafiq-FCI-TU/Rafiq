using AutoMapper;
using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Domain.Entities;

namespace Rafiq.Application.Profiles;

public class GamingProfile : Profile
{
    public GamingProfile()
    {
        CreateMap<GameLevel, GamingOverviewLevelDto>();
        CreateMap<Game, GamingOverviewGameDto>();
        CreateMap<GameCategory, GamingOverviewCategoryDto>();
    }
}
