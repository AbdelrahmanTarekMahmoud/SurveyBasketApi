namespace SurveyBasket.Api.Services
{
    public interface INotificationService
    {
        //Polls that will be started soon
        Task NewPollsNotification();
    }
}
