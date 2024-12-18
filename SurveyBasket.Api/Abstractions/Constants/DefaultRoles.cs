namespace SurveyBasket.Api.Abstractions.Constants
{
    //seeding of "AspNetRoles" Table 
    //seeding with 2 types (Admin & Member)
    public static class DefaultRoles
    {
        public const string Admin = nameof(Admin);
        public const string AdminRoleId = "0193c565-3bd9-75bb-b95b-4fc96d452676";
        public const string AdminConcurrenyStamp = "0193c565-3bd9-75bb-b95b-4fcb99ca9be0";




        public const string Member = nameof(Member);
        public const string MemberRoleId = "0193c565-3bd9-75bb-b95b-4fcadd48391a";
        public const string MemberConcurrenyStamp = "0193c565-3bd9-75bb-b95b-4fccd4b50282";
    }
}
