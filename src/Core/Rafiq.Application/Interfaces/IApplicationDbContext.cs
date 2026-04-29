using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafiq.Domain.Entities;
using Rafiq.Domain.Entities.Community;

namespace Rafiq.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<FamilyProfile> FamilyProfiles { get; }
    DbSet<SpecialistProfile> SpecialistProfiles { get; }
    DbSet<Patient> Patients { get; }
    DbSet<SpecialistPatient> SpecialistPatients { get; }
    DbSet<PatientSessionAttempt> PatientSessionAttempts { get; }
    DbSet<ChatMessage> ChatMessages { get; }
    DbSet<CommunityPost> CommunityPosts { get; }
    DbSet<Comment> Comments { get; }
    DbSet<PostReaction> PostReactions { get; }
    DbSet<CommentReaction> CommentReactions { get; }
    DbSet<Notification> Notifications { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
