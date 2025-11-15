using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities;

namespace Rafiq.Persistence.Data.Configurations
{
    public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            //builder.Property(a => a.FrequencyAnswer)
            //    .HasConversion<string>()
            //    .HasMaxLength(20);


            //builder.Property(a => a.DifficultyAnswer)
            //    .HasConversion<string>()
            //    .HasMaxLength(20);

            //// Relationship to Assessment(many-to - one)
            //builder.HasOne(a => a.Assessment)
            //    .WithMany(a => a.Answers)
            //    .HasForeignKey(a => a.AssessmentId)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Indexes (unique per assessment/question to prevent duplicates)
            builder.HasIndex(a => new { a.QuestionId }).IsUnique();
        }
    }
}
