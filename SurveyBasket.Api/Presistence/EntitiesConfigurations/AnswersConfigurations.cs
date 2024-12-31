namespace SurveyBasket.Api.Presistence.EntitiesConfigurations
{
    public class AnswersConfigurations : IEntityTypeConfiguration<Answer>
    {
        public void Configure(EntityTypeBuilder<Answer> builder)
        {
            //composite unique (the same answer cant be duplicated for same question)
            builder.HasIndex(x => new { x.QuestionId, x.Content }).IsUnique();
            builder.Property(x => x.Content).HasMaxLength(1000);



        }
    }
}
