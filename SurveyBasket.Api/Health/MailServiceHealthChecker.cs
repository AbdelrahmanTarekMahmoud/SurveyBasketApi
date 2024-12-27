using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SurveyBasket.Api.Health
{
    public class MailServiceHealthChecker(IOptions<MailSettings> mailSettings) : IHealthCheck
    {
        private readonly MailSettings _mailSettings = mailSettings.Value;
        //Here main job is try to connect to the mail service we use 
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.None ,cancellationToken);
                smtp.Authenticate(_mailSettings.UserName, _mailSettings.Password , cancellationToken);

                return await Task.FromResult(HealthCheckResult.Healthy());
            }
            catch(Exception ex)
            {
                return await Task.FromResult(HealthCheckResult.Unhealthy(exception : ex));
            }
        }
    }
}
