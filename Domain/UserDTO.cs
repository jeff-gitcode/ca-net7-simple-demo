namespace Domain
{
    public class UserDTO : BaseEntity
    {
        public string? Id { get; set; } = null;
        public string FirstName { get; init; } = null!;
        public string LastName { get; init; } = null!;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string? Token { get; init; } = null;
    }

    public class UserResponse
    {
        public string? Id { get; set; } = null;
        public string Email { get; init; } = null!;
        public string Password { get; init; } = null!;
        public string? Role { get; init; } = UserRoles.User;
        public string? Token { get; init; } = null;
    }
}