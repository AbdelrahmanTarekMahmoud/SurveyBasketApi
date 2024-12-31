namespace SurveyBasket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    //[EnableRateLimiting("CustomIpAdrressRateLimit")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging info Email : {email} , Password : {password}", request.Email, request.Password);
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();

        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
        {

            var Result = await _authService.RegisterAsync(request, cancellationToken);
            return Result.IsSuccess ? Ok() : Result.ToProblem();
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
        {

            var Result = await _authService.ConfirmEmailAsync(request);
            return Result.IsSuccess ? Ok() : Result.ToProblem();
        }

        [HttpPost("resend-confirm-email")]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendEmailConfirmationRequest request, CancellationToken cancellationToken)
        {

            var Result = await _authService.ResendEmailConfirmationAsync(request);
            return Result.IsSuccess ? Ok() : Result.ToProblem();
        }

        [HttpPost("send-forgetpasswordcode-email")]
        public async Task<IActionResult> SendForgetPasswordEmail([FromBody] ForgetPasswordRequest request, CancellationToken cancellationToken)
        {

            var Result = await _authService.SendForgetPasswordCode(request, cancellationToken);
            return Result.IsSuccess ? Ok() : Result.ToProblem();
        }


        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
        {

            var Result = await _authService.ResetPasswordForForgettingPassword(request, cancellationToken);
            return Result.IsSuccess ? Ok() : Result.ToProblem();
        }

        //[HttpGet("test")]
        //[EnableRateLimiting("SlidingWindowLimiter")]
        //public IActionResult TestConcurrentRateLimiter()
        //{
        //    Thread.Sleep(5000);
        //    return Ok();
        //}
    }
}
