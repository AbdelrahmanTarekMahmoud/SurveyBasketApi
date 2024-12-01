
namespace SurveyBasket.Api.Presistence.EntitiesConfigurations
{
    public class PollConfigurations : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(x => x.Title).IsUnique();
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Summary).IsRequired().HasMaxLength(1500);
        }
    }
}
