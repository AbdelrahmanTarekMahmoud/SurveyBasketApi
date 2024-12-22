

using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Api.Errors;
using SurveyBasket.Api.Helper;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SurveyBasket.Api.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager 
        ,IJwtProvider jwtProvider , ILogger<AuthService> logger , IEmailSender emailSender
        ,IHttpContextAccessor httpContextAccessor , ApplicationDbContext context) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ApplicationDbContext _context = context;

        public async Task<Result<AuthResponse>> GetTokenAsync(string Email, string Password, CancellationToken cancellationToken = default)
        {
            //Check user existance
            var user = await _userManager.FindByEmailAsync(Email);
            if (user is null) 
            { 
               return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            }

            //added in sec 25
            if(user.IsDisabled)
            {
                return Result.Failure<AuthResponse>(UserErrors.UserIsDisabled);
            }
            //we can check also that way instead of isNotAllowed
            //if(!user.EmailConfirmed) return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);


            ////Correct Password
            //var isValidPasssword = await _userManager.CheckPasswordAsync(user, Password);
            //if (!isValidPasssword) 
            //{ 
            //    return  Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);  
            //}
            var result = await _signInManager.PasswordSignInAsync(user , Password, false , true);
            
            if (result.Succeeded)
            {
                var UserRoles = await _userManager.GetRolesAsync(user);
                var Permissions = await _context.Roles
                    .Join(_context.RoleClaims
                    , role => role.Id, claim => claim.RoleId,
                    (role, claim) => new { role, claim }).
                    Where(AnonymousObj => UserRoles.Contains(AnonymousObj.role.Name))
                    .Select(AnonymousObj => AnonymousObj.claim.ClaimValue)
                    .Distinct()
                    .ToListAsync(cancellationToken);

                //Generate Token
                var (token, expireIn) = _jwtProvider.GenerateToken(user , UserRoles , Permissions);
                var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expireIn);
                return Result.Success<AuthResponse>(response);
            }
            //if code continues here there is 2 reasons
            //1-passowrd or email are invalid
            //2-User didnt confirm the email yet
            //3-User lockedout

            var error = result.IsNotAllowed ? UserErrors.EmailNotConfirmed
                : result.IsLockedOut ? UserErrors.UserIsLockedOut : UserErrors.InvalidCredentials;

            return Result.Failure<AuthResponse>(error);
            
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

                
                await SendConfirmationEmail(user, code);


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
                await _userManager.AddToRoleAsync(user, DefaultRoles.Member);
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

            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        //helper method
        public async Task SendConfirmationEmail(ApplicationUser user , string code)
        {
            //https:/./surveybasket.com
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                new Dictionary<string, string>
                {
                    {"{{name}}" , user.FirstName},
                        {"{{action_url}}" , $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
                });
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survey Basket Confirmation Email", emailBody)); 
            await Task.CompletedTask;
        }

        public async Task<Result> SendForgetPasswordCode(ForgetPasswordRequest request, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                //misleading
                return Result.Success();
            }

            if(!user.EmailConfirmed)
            {
                return Result.Failure(UserErrors.EmailNotConfirmed);
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Reset  Code is : {c}", code);

            await SendForgetPasswordCodeEmail(user, code);

            return Result.Success();
        }

        //helper method
        public async Task SendForgetPasswordCodeEmail(ApplicationUser user, string code)
        {
            //https:/./surveybasket.com
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
                new Dictionary<string, string>
                {
                    {"{{name}}" , user.FirstName},
                        {"{{action_url}}" , $"{origin}/auth/forgetPassword?email={user.Email}&code={code}"}
                });
            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "Survey Basket PasswordReset", emailBody));
            await Task.CompletedTask;
        }

        public async Task<Result> ResetPasswordForForgettingPassword(ResetPasswordRequest request, CancellationToken cancellationToken = default)
        {
          
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null || !user.EmailConfirmed)
            {
                Result.Failure(UserErrors.InvalidCode);
            }

            IdentityResult result;
            try
            {
                var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
                result = await _userManager.ResetPasswordAsync(user, code, request.NewPassword);
            }
            catch(FormatException)
            {
                result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
            }
            if (result.Succeeded) return Result.Success();

            var error = result.Errors.FirstOrDefault();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
        }
    }
}