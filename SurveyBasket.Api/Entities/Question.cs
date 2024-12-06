namespace SurveyBasket.Api.Entities
{
    public sealed class Question : AuditableEntity
    {
        //primary Key
        public int Id { get; set; }
        //The Content of the question (question body)
        public string Content { get; set; } = string.Empty;

        //for soft delete no need to actually delete 
        public bool isActive { get; set; } = true;

        //Foreign Key
        public int PollId { get; set; }

        //Navigation Property
        public Poll Poll { get; set; } = default!;
        public ICollection<Answer> Answers { get; set; } = [];
    }
}
