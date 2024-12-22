//mapster global mapping
namespace SurveyBasket.Api.Mapping
{
    public class MappingConfigurations : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            //we use this if DTOS names changes from the actual data
            //for example (poll response title we changes it to header)
            //but in Poll in entities its still title
            //config.NewConfig<Poll, PollResponse>()
            //     .Map(dest => dest.newName, src => src.OriginalName);
            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));


            config.NewConfig<(ApplicationUser user, IList<string> role), UserResponse>()
                .Map(dest => dest, src => src.user)
                .Map(dest => dest.Roles, src => src.role);

            //For Creating User 
            config.NewConfig<CreateUserRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email)
                .Map(dest => dest.EmailConfirmed, src => true);
                
                

        }
    }
}
