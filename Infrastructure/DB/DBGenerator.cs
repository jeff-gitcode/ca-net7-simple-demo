using Domain;

using Infrastructure.Authentication;

using Microsoft.AspNetCore.Identity;

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
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await SeedAsync();
    }

    public async Task SeedAsync()
    {
        try
        {
            // Default roles
            var administratorRole = new IdentityRole("Admin");

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await _roleManager.CreateAsync(administratorRole);
            }

            // Default Admin user
            var administrator = new ApplicationUser { UserName = "admin@localhost", Email = "admin@localhost" };

            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Administrator1!");
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }

            // Default data
            // Seed, if necessary
            if (!_context.Users.Any())
            {
                _context.Users.Add(new UserDTO
                {
                    Id = Guid.NewGuid().ToString(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@email.com",
                    Password = "password",
                    Token = "token"
                });

                await _context.SaveChangesAsync();
            }
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
