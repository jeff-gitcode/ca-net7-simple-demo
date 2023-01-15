namespace Domain
{
    public class LoginDTO
    {
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string Role { get; set; } = UserRoles.User;
        public string? Token { get; set; } = null;
    }

    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }
}