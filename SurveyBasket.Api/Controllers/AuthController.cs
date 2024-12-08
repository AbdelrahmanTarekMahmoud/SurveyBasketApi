using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace SurveyBasket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService , ILogger<AuthController> logger) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly ILogger<AuthController> _logger = logger;

        [HttpPost("")]
        public async Task<IActionResult> LoginAsync(LoginRequest request , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Logging info Email : {email} , Password : {password}" , request.Email, request.Password);
            var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            if (authResult.IsFailure) { return authResult.ToProblem(); }
            return Ok(authResult.Value);
        }

    }
}
