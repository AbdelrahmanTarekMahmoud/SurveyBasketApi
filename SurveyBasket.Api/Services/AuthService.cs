

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Helper;
using System.Text;

namespace SurveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager 
        ,IJwtProvider jwtProvider , ILogger<AuthService> logger , IEmailSender emailSender
        ,IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<AuthResponse>> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
        {
            //Check user existance
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null) 
            { 
               return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            }

            //we can check also that way instead of isNotAllowed
            //if(!user.EmailConfirmed) return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);


            ////Correct Password
            //var isValidPasssword = await _userManager.CheckPasswordAsync(user, Password);
            //if (!isValidPasssword) 
            //{ 
            //    return  Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);  
            //}
            var result = await _signInManager.PasswordSignInAsync(user , Password, false , false);
            if (result.Succeeded)
            {
                //Generate Token
                var (token, expireIn) = _jwtProvider.GenerateToken(user);
                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireIn);
                return Result.Success<AuthResponse>(response);
            }
            //if code continues here there is 2 reasons
            //1-passowrd or email are invalid
            //2-User didnt confirm the email yet

            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);
            
        }

        public async Task<Result> RegisterAsync(RegisterRequest registerRequest, CancellationToken cancellationToken = default)
        {
            var IsEmailExist = await _userManager.Users.AnyAsync(x => x.Email == registerRequest.Email , cancellationToken);
            if(IsEmailExist)
            {
                return Result.Failure(UserErrors.AlreadyRegisteredEmail); 
            }

            var user = new ApplicationUser
            {
                Email = registerRequest.Email,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.Email
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            //send confirmation code
            if(result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                _logger.LogInformation("Conformation Code is : {c}" , code);

                //https:/./surveybasket.com
                var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

                var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                    new Dictionary<string, string>
                    {
                        {"{{name}}" , user.FirstName},
                        {"{{action_url}}" , $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
                    });
                await _emailSender.SendEmailAsync(user.Email!, "Survey Basket Confirmation Email", emailBody);


                return Result.Success();
            }
            //get only the first error (identity result not our built result class)
            var error = result.Errors.First();


            return Result.Failure(new Error(error.Code , error.Description , StatusCodes.Status400BadRequest  ));
        }


        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            var user = await _userManager.FindByIdAsync(confirmEmailRequest.UserId);
            if (user is null)
            {
                return Result.Failure(UserErrors.InvalidConfirmationCode);
            }

            //user found with id
            //check if the user already confirmed
            if (user.EmailConfirmed) Result.Failure(UserErrors.UserAlreadyConfirmed);

            //confirm the code itself
            var code = confirmEmailRequest.Code;
            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            }
            catch (FormatException)
            {
                return Result.Failure(UserErrors.InvalidConfirmationCode);
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                return Result.Success();
            }
            //get only the first error (identity result not our built result class)
            var error = result.Errors.First();


            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        public async Task<Result> ResendEmailConfirmationAsync(ResendEmailConfirmationRequest resendEmailConfirmationRequest)
        {
            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationRequest.Email);
            //dont give the user info about "there is no existed email with this email"
            if(user is null)
            {
                return Result.Success();
            }
            if (user.EmailConfirmed) return Result.Failure(UserErrors.UserAlreadyConfirmed);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Conformation Code is : {c}", code);

            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                        {"{{name}}" , user.FirstName},
                        {"{{action_url}}" , $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
                });
            await _emailSender.SendEmailAsync(user.Email!, "Survey Basket Confirmation Email", emailBody);

            return Result.Success();
        }
    }
}