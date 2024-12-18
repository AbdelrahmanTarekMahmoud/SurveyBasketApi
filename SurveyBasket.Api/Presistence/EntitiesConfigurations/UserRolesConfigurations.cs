
namespace SurveyBasket.Api.Presistence.EntitiesConfigurations
{
    public class UserRolesConfigurations : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(new IdentityUserRole<string>()
            {
                UserId = DefaultUsers.AdminId,
                RoleId = DefaultRoles.AdminRoleId
            });

        }
    }
}
