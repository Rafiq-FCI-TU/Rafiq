using System.Text.Json.Serialization;

namespace Rafiq.Application.Dtos;
public class ResponseDto
{
    public string Message { get; set; } = null!;
    public bool IsAuthenticated { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<string> Roles { get; set; } = [];
    public string Token { get; set; } = null!;
    public DateTime ExpiresOn { get; set; }

    [JsonIgnore]
    public string? RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }
}