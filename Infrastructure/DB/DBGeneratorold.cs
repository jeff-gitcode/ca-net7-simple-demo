// using Domain;

// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;

// namespace Infrastructure.DB;

// public class DBGenerator
// {
//     public static void Initialize(IServiceProvider serviceProvider)
//     {
//         using (var context = new DemoDBContext(
//             serviceProvider.GetRequiredService<DbContextOptions<DemoDBContext>>()))
//         {
//             // Default roles
//             var administratorRole = new IdentityRole("Admin");

//             if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
//             {
//                 await _roleManager.CreateAsync(administratorRole);
//             }

//             // Look for any user.
//             if (context.Users.Any())
//             {
//                 return;   // DB has been seeded
//             }

//             context.Users.AddRange(
//                 new UserDTO
//                 {
//                     Id = Guid.NewGuid().ToString(),
//                     FirstName = "John",
//                     LastName = "Doe",
//                     Email = "john.doe@email.com",
//                     Password = "password",
//                     Token = "token"
//                 });

//             context.SaveChanges();
//         }
//     }
// }