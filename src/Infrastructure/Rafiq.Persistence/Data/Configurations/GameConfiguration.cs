namespace Rafiq.Persistence.Data.Configurations;

public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(g => g.Id);

        builder.Property(g => g.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(g => g.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        // One Game → Many Levels
        builder.HasMany(g => g.Levels)
            .WithOne(l => l.Game)
            .HasForeignKey(l => l.GameId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
