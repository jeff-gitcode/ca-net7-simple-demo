using Grpc.Core;
using Grpc.Net.Client;

using Presentation.Grpc;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var channel = GrpcChannel.ForAddress("https://localhost:7128");

try
{
    // Say hello to the gRPC server
    var client = new Greeter.GreeterClient(channel);
    var reply = await client.SayHelloAsync(
                  new HelloRequest { Name = "GreeterClient" });
    Console.WriteLine("Greeting: " + reply.Message);

    // User CRUD operations to gRPC server
    var userClient = new UsersService.UsersServiceClient(channel);

    var userReply = await userClient.CreateUserAsync(new User
    {
        FirstName = "John",
        LastName = "Doe",
        Email = "john.doe@grpclient.com",
        Password = "123456",
        Token = "123456"
    });

    Console.WriteLine("User created: " + userReply);

    var user = await userClient.GetUserByIdAsync(new UserId { Id = userReply.Id });

    Console.WriteLine("User retrieved by id: " + user);

    user.FirstName = "Jane Update";

    var userUpdateReply = await userClient.UpdateUserAsync(user);

    Console.WriteLine("User updated: " + userUpdateReply);

    var userDeleteReply = await userClient.DeleteUserAsync(new UserId { Id = userReply.Id });

    Console.WriteLine("User deleted: " + userDeleteReply);

    var userList = await userClient.GetAllUsersAsync(new Empty());

    Console.WriteLine("User list: " + userList);

}
catch (System.Exception ex)
{
    throw ex;
}

