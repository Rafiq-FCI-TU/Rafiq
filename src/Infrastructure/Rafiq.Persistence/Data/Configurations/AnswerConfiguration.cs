using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities;

namespace Rafiq.Persistence.Data.Configurations;
public class AnswerConfiguration : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.Property(a => a.SelectedOption)
            .HasMaxLength(20);

        // Indexes (unique per assessment/question to prevent duplicates)
        builder.HasIndex(a => new { a.QuestionId }).IsUnique();
    }
}
