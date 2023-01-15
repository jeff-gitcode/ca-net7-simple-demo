namespace Presentation.WebApi.Authentication;

public record LoginRequest(string Email, string Password);

public record RegisterRequest(string Email, string Password, string FirstName, string LastName);

public record LoginResponse(string Email, string Password, string FirstName, string LastName, string Token);

public record RegisterResponse(Guid Id, string Email, string Password, string FirstName, string LastName, string Token);

