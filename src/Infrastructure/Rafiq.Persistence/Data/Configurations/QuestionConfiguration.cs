using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafiq.Domain.Entities;

namespace Rafiq.Persistence.Data.Configurations;
public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.Property(q => q.Text)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(q => q.AnswerType)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(q => q.Category)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(q => q.Order)
            .IsRequired();
    }
}
