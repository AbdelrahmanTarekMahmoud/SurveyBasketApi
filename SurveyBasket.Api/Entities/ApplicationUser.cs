// this is a extentsion for the identity table (we added firstName , lastName) 

/*
 * 
 * 
  {
  "email": "admin@test.com",
  "password": "P@assword123" 
  }
 */
namespace SurveyBasket.Api.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public bool IsDisabled { get; set; }

    }
}
