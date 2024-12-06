namespace SurveyBasket.Api.Presistence.EntitiesConfigurations
{
    public class VoteAnswerConfigurations : IEntityTypeConfiguration<VoteAnswer>
    {
        public void Configure(EntityTypeBuilder<VoteAnswer> builder)
        {
            //same "vote" cannot have the same "question"
            builder.HasIndex(x => new { x.QuestionId, x.VoteId }).IsUnique();
        }
    }
}
