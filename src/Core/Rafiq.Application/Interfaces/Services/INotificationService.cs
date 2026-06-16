using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task SaveAsync(string userId, string title, string message,
                       string type, string? payload = null,
                       CancellationToken cancellationToken = default);
    }
}
