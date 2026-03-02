namespace Rafiq.Domain.Entities;
public class PasswordResetToken
{
    public int Id { get; set; }  // PK required by EF Core
    public string Otp { get; set; } = string.Empty;
    public DateTime ExpiresOn { get; set; } = DateTime.UtcNow.AddMinutes(10);
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public bool IsVerified { get; set; } = false;
    public bool IsExpired => DateTime.UtcNow > ExpiresOn;
    public bool IsActive => !IsExpired && !IsVerified;
}