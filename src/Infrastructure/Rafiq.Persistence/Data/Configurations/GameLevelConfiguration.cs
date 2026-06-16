namespace Rafiq.Persistence.Data.Configurations;

public class GameLevelConfiguration : IEntityTypeConfiguration<GameLevel>
{
    public void Configure(EntityTypeBuilder<GameLevel> builder)
    {
        builder.HasKey(gl => gl.Id);

        //builder.Property(gl => gl.RequiredScore)
        //    .IsRequired()
        //    .HasDefaultValue(0);

        //builder.Property(gl => gl.DifficultyLevel)
        //    .IsRequired();

    }
}
