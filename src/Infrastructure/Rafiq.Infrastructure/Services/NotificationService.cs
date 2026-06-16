using Rafiq.Application.Interfaces;
using Rafiq.Application.Interfaces.Services;
using Rafiq.Domain.Entities;

namespace Rafiq.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IApplicationDbContext _db;

        public NotificationService(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task SaveAsync(string userId, string title, string message,
                                    string type, string? payload = null,
                                    CancellationToken cancellationToken = default)
        {
            var notification = new Notification
            {
                UserId              = userId,
                Title               = title,
                Message             = message,
                TypeofNotification  = type,
                Payload             = payload,
                CreatedAt           = DateTime.UtcNow
            };

            await _db.Notifications.AddAsync(notification, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}