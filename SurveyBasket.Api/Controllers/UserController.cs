
using SurveyBasket.Api.Authentication.Filters;

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

        //25
        [HttpGet("user-data")]
        [HasPermission(Permissions.GetUsers)]
        public async Task<IActionResult> GetAllUsersDetails(CancellationToken cancellationToken)
        {
            var result = await _userService.GetAllUsersDetailsAsync(cancellationToken);
            return Ok(result);
        }
 

        [HttpGet("user/{id}")]
        [HasPermission(Permissions.GetUsers)]
        public async Task<IActionResult> GetUserData([FromRoute] string id ,CancellationToken cancellationToken)
        {
            var result = await _userService.GetUserDetailsAsync(id , cancellationToken);
            return result.IsSuccess? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        [HasPermission(Permissions.AddUser)]
        public async Task<IActionResult> CreateNewUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
        {
            var result = await _userService.CreateNewUserAsync(request, cancellationToken);
            return result.IsSuccess ? CreatedAtAction(nameof(GetUserData) , new {result.Value.Id} , result.Value) : result.ToProblem();
        }


        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdateUser)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request,[FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _userService.UpdateUserAsync(request, id, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        [HttpPut("toggle/{id}")]
        [HasPermission(Permissions.UpdateUser)]
        public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _userService.ToggleUserStatusAsync(id, cancellationToken);

            return result.IsSuccess? NoContent() : result.ToProblem();  
        }

        [HttpPut("lockout/{id}")]
        [HasPermission(Permissions.UpdateUser)]
        public async Task<IActionResult> UnlockUser([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _userService.UnLockUser(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
