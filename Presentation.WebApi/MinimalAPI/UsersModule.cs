using Application.Interface.API;

using Carter;

using Domain;

namespace Presentation.WebApi.MinimalAPI;

public class UsersModule : CarterModule
{
    public UsersModule() : base("api/minimal/users")
    {

    }

    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", async (IUserUseCase userUseCase) =>
        {
            var users = await userUseCase.GetAllUsers();
            return Results.Ok(users);
        }).RequireAuthorization(UserRoles.Admin);

        app.MapGet("/{id}", async (IUserUseCase userUseCase, string id) =>
        {
            var user = await userUseCase.GetUserById(id);
            return Results.Ok(user);
        }).RequireAuthorization();

        app.MapPost("/", async (IUserUseCase userUseCase, UserDTO tempUser) =>
        {
            var user = await userUseCase.CreateUser(tempUser);
            return Results.Ok(user);
        }).RequireAuthorization();

        app.MapPut("/", async (IUserUseCase userUseCase, UserDTO tempUser) =>
        {
            var user = await userUseCase.UpdateUser(tempUser);
            return Results.Ok(user);
        }).RequireAuthorization();

        app.MapDelete("/{id}", async (IUserUseCase userUseCase, string id) =>
        {
            var user = await userUseCase.DeleteUser(id);
            return Results.Ok(user);
        }).RequireAuthorization();
    }
}
