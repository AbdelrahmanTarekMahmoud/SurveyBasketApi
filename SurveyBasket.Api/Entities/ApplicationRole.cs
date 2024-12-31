namespace SurveyBasket.Api.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public bool IsDefault { get; set; }
        //soft delete
        public bool IsDeleted { get; set; }
    }
}
