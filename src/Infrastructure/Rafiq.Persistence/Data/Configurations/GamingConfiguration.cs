namespace Rafiq.Persistence.Data.Configurations;
public class GamingConfiguration : IEntityTypeConfiguration<Gaming>
{
    public void Configure(EntityTypeBuilder<Gaming> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.TotalScore)
            .HasDefaultValue(0);

        builder.Property(g => g.CreatedAt)
            .HasDefaultValueSql("GETDATE()");
    }
}
