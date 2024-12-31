namespace SurveyBasket.Api.Presistence.EntitiesConfigurations
{
    public class VoteConfigurations : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> builder)
        {
            //same "user" cannot apply more than one vote in the same "poll"
            builder.HasIndex(x => new { x.PollId, x.UserId }).IsUnique();
        }
    }
}
