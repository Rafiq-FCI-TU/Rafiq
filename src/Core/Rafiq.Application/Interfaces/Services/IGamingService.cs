using Rafiq.Application.Dtos.GamingDtos;
using Rafiq.Domain.Enums;

namespace Rafiq.Application.Interfaces.Services;
public interface IGamingService
{
    Task<GamingDto> InitializeGamingAsync(int patientId);
    Task<GamingDto> GetGamingByPatientAsync(int patientId);
    Task<IReadOnlyList<GameDto>> GetGamesByCategoryAsync(int categoryId);
    Task<GameProgressDto> RecordGameProgressAsync(CreateGameProgressDto dto);
    Task<IReadOnlyList<GameProgressDto>> GetGameProgressAsync(int gameId, int patientId);
    Task UpdateGameStatusAsync(int gameId, GameStatus status);
}
