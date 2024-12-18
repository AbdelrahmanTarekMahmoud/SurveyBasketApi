
namespace SurveyBasket.Api.Authentication.Filters
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        //requirement is the param "Permissions.GetPolls" inside [HasPermission(Permissions.GetPolls)] 
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var user = context.User.Identity;
            if(user == null || !user.IsAuthenticated)
            {
                return; 
            }
            var isUserAppliesReqs = context.User.Claims
                .Any(x => x.Value == requirement.Permission);

            if(!isUserAppliesReqs)
            {
                return;
            }

            context.Succeed(requirement);
            return;
        }
    }
}
