
namespace SurveyBasket.Api.Presistence.EntitiesConfigurations
{
    public class QuestionConfigurations : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.Property(x => x.Content).IsRequired().HasMaxLength(1000);
            //same poll cannot has the same qustions duplicated
            builder.HasIndex(x => new { x.PollId, x.Content }).IsUnique();
        }
    }
}
