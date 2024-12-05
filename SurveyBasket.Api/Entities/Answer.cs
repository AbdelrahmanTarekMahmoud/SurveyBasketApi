namespace SurveyBasket.Api.Entities
{
    public class Answer 
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;

        //for soft delete no need to actually delete
        public bool isActive { get; set; } = true;

        //Foreign Key
        public int QuestionId { get; set; }

        //navigation property
        public Question Question { get; set; } = default!;
         
    }
}
