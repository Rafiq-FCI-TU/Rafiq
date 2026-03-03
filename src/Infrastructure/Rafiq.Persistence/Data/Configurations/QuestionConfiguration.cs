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
               .HasConversion<string>()
               .HasMaxLength(20);

        builder.Property(q => q.Order)
               .IsRequired();
    }

}
