namespace SurveyBasket.Api.Entities
{
    public class Answer 
    {
        //primary Key
        public int Id { get; set; }
        //Content of the answers (for example : 1- a  , 2- b , 3- c)
        public string Content { get; set; } = string.Empty;

        //for soft delete no need to actually delete
        public bool isActive { get; set; } = true;

        //Foreign Key
        public int QuestionId { get; set; }

        //navigation property
        public Question Question { get; set; } = default!;
         
    }
}
