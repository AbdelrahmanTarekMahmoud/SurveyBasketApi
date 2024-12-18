namespace SurveyBasket.Api.Abstractions.Constants
{
    public static class Permissions
    {
        public static string Type { get; } = "Permissions";

        //permissions 
        public const string GetPolls = "Polls:Read";
        public const string AddPolls = "Polls:Add";
        public const string UpdatePolls = "Polls:Update";
        public const string DeletePolls = "Polls:Delete";

        public const string GetQuestions = "Questions:Read";
        public const string AddQuestions = "Questions:Add";
        public const string UpdateQuestions = "Questions:Update";

        public const string GetUsers = "Users:Read";
        public const string AddUser = "Users:Add";
        public const string UpdateUser = "Users:Update";

        public const string GetRoles = "Roles:Read";
        public const string AddRoles = "Roles:Add";
        public const string UpdateRoles = "Roles:Update";

        public const string Results = "Results:Read";

        public static IList<string?> GetAllPermissions()
        {
            return typeof(Permissions).GetFields().Select(x => x.GetValue(x) as string).ToList();
        }
    }
}
