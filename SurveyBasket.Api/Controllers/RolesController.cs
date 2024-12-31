
using SurveyBasket.Api.Authentication.Filters;
using SurveyBasket.Api.Contracts.Roles;

namespace SurveyBasket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController(IRoleService roleService) : ControllerBase
    {
        private readonly IRoleService _roleService = roleService;

        [HttpGet("all")]
        [HasPermission(Permissions.GetRoles)]
        public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
        {
            var Roles = await _roleService.GetAllRolesAsync(cancellationToken);
            return Ok(Roles);
        }


        [HttpGet("active")]
        [HasPermission(Permissions.GetRoles)]
        public async Task<IActionResult> GetActiveRoles(CancellationToken cancellationToken)
        {
            var Roles = await _roleService.GetActiveRolesAsync(cancellationToken);
            return Ok(Roles);
        }

        [HttpGet("{id}")]
        [HasPermission(Permissions.GetRoles)]
        public async Task<IActionResult> GetRoleById([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _roleService.GetRoleByIdAsync(id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }


        [HttpPost("")]
        [HasPermission(Permissions.AddRoles)]
        public async Task<IActionResult> AddRole([FromBody] RolesRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.AddRoleAsync(request, cancellationToken);

            return result.IsSuccess ?
                CreatedAtAction(nameof(GetRoleById), new { result.Value.Id }, result.Value)
                : result.ToProblem();
        }


        [HttpPut("{id}")]
        [HasPermission(Permissions.UpdateRoles)]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] RolesRequest request, CancellationToken cancellationToken)
        {
            var result = await _roleService.UpdateRoleAsync(id, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }


        [HttpPut("{id}/toggle")]
        [HasPermission(Permissions.UpdateRoles)]
        public async Task<IActionResult> ToggleIsDeletedStatus([FromRoute] string id, CancellationToken cancellationToken)
        {
            var result = await _roleService.ToggleIsDeletedStatusAsync(id, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
    }
}
