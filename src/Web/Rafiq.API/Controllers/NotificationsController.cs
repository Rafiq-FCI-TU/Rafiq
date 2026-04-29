using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rafiq.API.Extensions;
using Rafiq.Application.Interfaces;
using System.Security.Claims;

namespace Rafiq.API.Controllers
{
    /// <summary>
    /// Handles notification retrieval and read-status management for authenticated users.
    /// </summary>

    public class NotificationsController : BaseController
    {
        private readonly IApplicationDbContext _db;

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public NotificationsController(IApplicationDbContext db)
        {
            _db = db;
        }

        #region Get Notifications

        /// <summary>
        /// Gets all notifications for the authenticated user, ordered by newest first.
        /// </summary>
        /// <returns>List of notifications.</returns>
        /// <response code="200">Notifications retrieved successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAll()
        {
            var notifications = await _db.Notifications
                .Where(n => n.UserId == UserId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.Id,
                    n.Title,
                    n.Message,
                    n.TypeofNotification,
                    n.Payload,
                    n.IsRead,
                    n.CreatedAt
                })
                .ToListAsync();

            return Ok(notifications);
        }

        /// <summary>
        /// Gets the count of unread notifications for the bell icon badge.
        /// </summary>
        /// <returns>Unread notification count.</returns>
        /// <response code="200">Count retrieved successfully.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpGet("unread-count")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> UnreadCount()
        {
            var count = await _db.Notifications
                .CountAsync(n => n.UserId == UserId && !n.IsRead);

            return Ok(new { count });
        }

        #endregion

        #region Mark as Read

        /// <summary>
        /// Marks a single notification as read.
        /// </summary>
        /// <param name="id">The notification ID.</param>
        /// <returns>No content.</returns>
        /// <response code="204">Marked as read successfully.</response>
        /// <response code="404">Notification not found.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpPut("{id}/read")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var notification = await _db.Notifications
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == UserId);

            if (notification is null)
                return NotFound();

            notification.IsRead = true;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Marks all unread notifications as read for the authenticated user.
        /// </summary>
        /// <returns>No content.</returns>
        /// <response code="204">All notifications marked as read.</response>
        /// <response code="401">User is not authenticated.</response>
        [HttpPut("read-all")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            await _db.Notifications
                .Where(n => n.UserId == UserId && !n.IsRead)
                .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true));

            return NoContent();
        }

        #endregion
    }
}