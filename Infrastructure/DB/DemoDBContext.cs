using Domain;

using Duende.IdentityServer.EntityFramework.Options;

using Infrastructure.Authentication;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Infrastructure.DB
{
    public interface IDbContext : IDisposable
    {
        DbSet<UserDTO> Users { get; set; }

        Task<int> SaveChangesAsync();
    }

    public class DemoDBContext : ApiAuthorizationDbContext<ApplicationUser>, IDbContext
    {
        public DemoDBContext(DbContextOptions<DemoDBContext> options, IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<UserDTO> Users { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
