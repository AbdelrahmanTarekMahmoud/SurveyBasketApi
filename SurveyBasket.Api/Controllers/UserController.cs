
namespace SurveyBasket.Api.Controllers
{
    [Route("me")]
    [ApiController]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("")]
        public async Task<IActionResult> GetUserProfile()
        {
            var result = await _userService.GetUserProfileAsync(User.GetUserId());
            return Ok(result.Value);
        }



        [HttpPut("profile-update")]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileUpdateRequest request , CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateUserProfileAsync(User.GetUserId(), request , cancellationToken);
            return NoContent();
        }


        [HttpPut("profile-password-change")]
        public async Task<IActionResult> ChangeUserPassword([FromBody] UserChangePasswordRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request, cancellationToken);
            
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
