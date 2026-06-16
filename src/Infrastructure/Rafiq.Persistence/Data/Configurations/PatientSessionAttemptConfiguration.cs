using Rafiq.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Persistence.Data.Configurations
{
    public class PatientSessionAttemptConfiguration: IEntityTypeConfiguration<PatientSessionAttempt>
    {
        public void Configure(EntityTypeBuilder<PatientSessionAttempt> builder)
        {
            builder.Property(s => s.Duration)
           .HasConversion<TimeSpan>();

            builder.Property(a => a.Status)
           .IsRequired()
           .HasConversion<string>()
           .HasMaxLength(20)
           .HasDefaultValue(AttemptStatus.PendingReview);



            builder.HasOne(a => a.Session)
           .WithMany(s => s.PatientAttempts)
           .HasForeignKey(a => a.SessionId)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Patient)
                .WithMany(p => p.SessionAttempts)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new { a.PatientId, a.SessionId });

        }





    }
}
