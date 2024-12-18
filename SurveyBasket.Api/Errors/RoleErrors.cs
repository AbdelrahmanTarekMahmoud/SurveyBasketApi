namespace SurveyBasket.Api.Errors
{
    public class RoleErrors
    {
        public static readonly Error RoleNotFound =
         new("Role.RoleNotFound", "Role with this id does not exist", StatusCodes.Status404NotFound);

        public static readonly Error RoleDuplicated =
         new("Role.RoleDuplicated", "Role with the same name is already exist", StatusCodes.Status409Conflict);

        public static readonly Error PermissionsInvalid =
         new("Role.PermissionsInvalid", "Permission in request does not exist in database permissions", StatusCodes.Status404NotFound);
    }
}
