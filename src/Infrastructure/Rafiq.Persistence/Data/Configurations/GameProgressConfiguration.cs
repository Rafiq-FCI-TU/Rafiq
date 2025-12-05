using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rafiq.Persistence.Data.Configurations
{
    public class GameProgressConfiguration : IEntityTypeConfiguration<GameProgress>
    {
        public void Configure(EntityTypeBuilder<GameProgress> builder)
        {
            builder.HasKey(gp => gp.Id);

            builder.Property(gp => gp.AchievedPoints)
                .HasDefaultValue(0);

            builder.Property(gp => gp.TimePlayed)
                .HasDefaultValue(0);

            builder.Property(gp => gp.PlayedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(gp => gp.Completed)
                .HasDefaultValue(false);

            // Relation with Game
            builder.HasOne(gp => gp.Game)
                .WithMany(g => g.ProgressRecords)
                .HasForeignKey(gp => gp.GameId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relation with Patient
            builder.HasOne(gp => gp.Patient)
                .WithMany()
                .HasForeignKey(gp => gp.PatientId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
