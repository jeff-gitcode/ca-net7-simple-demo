using Domain;

using Infrastructure.Authentication;
using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DB;

public class DBGenerator
{
    private readonly ILogger<DBGenerator> _logger;
    private readonly DemoDBContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DBGenerator(ILogger<DBGenerator> logger, DemoDBContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        Guard.Against.Null(context, nameof(context));
        Guard.Against.Null(userManager, nameof(userManager));
        Guard.Against.Null(roleManager, nameof(roleManager));

        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        _logger.LogInformation("Initializing database...");
        await _context.Database.EnsureCreatedAsync();
        await _context.SaveChangesAsync();

        await SeedAsync();
    }

    public async Task SeedAsync()
    {
        try
        {
            // Default roles
            // var administratorRole = new IdentityRole();

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            // if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            // {
            //     _logger.LogInformation("Creating default roles...");
            //     await _roleManager.CreateAsync(administratorRole);
            //     await _context.SaveChangesAsync();

            //     var userRole = new IdentityRole("User");
            //     await _roleManager.CreateAsync(userRole);
            //     await _context.SaveChangesAsync();
            // }

            // Default Admin user
            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };
            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                var user = new ApplicationUser
                {
                    UserName = administrator.UserName,
                    Email = administrator.Email,
                    // EmailConfirmed = true,
                };


                var identityResult = await _userManager.CreateAsync(user, "P@ssword1");
                var existingUser = await _userManager.FindByNameAsync(user.Email);

                _logger.LogInformation("Creating default administrator... {0}", existingUser);
                if (identityResult.Succeeded)
                {
                    if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    {
                        _logger.LogInformation("Creating default administrator role...");
                        await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                        var roles = await _userManager.GetRolesAsync(user);
                        _logger.LogInformation("Creating default administrator role... {0}", roles);
                    }
                }
                await _context.SaveChangesAsync();
            }
            // var administrator = new ApplicationUser { UserName = "admin@localhost", Email = "admin@localhost" };

            // if (!_userManager.Users.Any(u => u.UserName == administrator.UserName))
            // {
            //     // Create the Admin account and apply the Administrator role                
            //     _logger.LogInformation("Creating default administrator...");
            //     await _userManager.CreateAsync(administrator, "password");
            //     await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            //     await _context.SaveChangesAsync();
            // }

            _logger.LogInformation("Seeding database...");

            // Default data
            // Seed, if necessary
            // if (!_context.Customer.Any())
            // {
            //     _context.Customer.Add(new UserDTO
            //     {
            //         Id = Guid.NewGuid().ToString(),
            //         FirstName = "John",
            //         LastName = "Doe",
            //         Email = "john.doe@email.com",
            //         Password = "password",
            //         Token = "token"
            //     });

            //     await _context.SaveChangesAsync();
            // }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
