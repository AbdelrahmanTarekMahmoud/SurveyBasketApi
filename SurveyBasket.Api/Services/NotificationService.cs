
using SurveyBasket.Api.Helper;

namespace SurveyBasket.Api.Services
{
    public class NotificationService(ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        IHttpContextAccessor httpContextAccessor, IEmailSender emailSender) : INotificationService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IEmailSender _emailSender = emailSender;

        public async Task NewPollsNotification()
        {
            var NewPolls = await _context.polls.Where(x => x.IsPublished &&
            x.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow)).AsNoTracking().ToListAsync();


            //TODO SEND TO SPECIFIC USERS
            //var users = await _userManager.Users.ToListAsync();
            var users = await _userManager.GetUsersInRoleAsync(DefaultRoles.Member);
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
            foreach (var poll in NewPolls)
            {
                foreach (var user in users)
                {
                    var placeholder = new Dictionary<string, string>
                    {
                        {"{{name}}" , $"{user.FirstName} {user.LastName}"},
                        {"{{pollTill}}" , poll.Title},
                        {"{{endDate}}" , poll.EndsAt.ToString()},
                        {"{{url}}" ,$"{origin}/polls/notification?pollId={poll.Id}"}
                    };

                    var body = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholder);
                    await _emailSender.SendEmailAsync(user.Email!, "Survey BasNew Polls Notification", body);
                }
            }

        }

    }
}
