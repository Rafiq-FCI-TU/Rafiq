namespace Rafiq.Persistence.Data.Configurations;

public class GameCategoryConfiguration : IEntityTypeConfiguration<GameCategory>
{
    public void Configure(EntityTypeBuilder<GameCategory> builder)
    {
        builder.HasKey(gc => gc.Id);

        builder.Property(gc => gc.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(gc => gc.InitialScore)
            .HasDefaultValue(0);

        // One Category → Many Games
        builder.HasMany(gc => gc.Games)
            .WithOne(g => g.Category)
            .HasForeignKey(g => g.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
