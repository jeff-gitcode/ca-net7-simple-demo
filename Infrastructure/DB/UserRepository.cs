using Application.SPI;

using Domain;

using Infrastructure.DB;

using MediatR;


namespace WebAPI.Infrastructure.DB;

public class UserRepository : IUserRepository
{
    private readonly IDbContext _context;
    // private readonly static List<UserDTO> _users = new();

    public UserRepository(IDbContext context) => _context = context;

    public async Task<List<UserDTO>> GetAllAsync()
    {
        // Get rid of async warning for now
        await Task.CompletedTask;

        // return _users;
        return _context.Users.ToList();
    }

    public async Task<UserDTO> GetByIdAsync(string id)
    {
        await Task.CompletedTask;

        return _context.Users.FirstOrDefault(x => x.Id == id);
        // return _users.FirstOrDefault(x => x.Id.ToString() == id);
    }

    public async Task<UserDTO> AddAsync(UserDTO user)
    {
        user.Id = Guid.NewGuid().ToString();
        _context.Users.Add(user);
        var result = await _context.SaveChangesAsync();

        return user;

        // await Task.CompletedTask;
        // tempUser.Id = Guid.NewGuid().ToString();
        // _users.Add(tempUser);
        // return tempUser;
    }

    public async Task<UserDTO> UpdateAsync(UserDTO user)
    {
        _context.Users.Update(user);
        var result = await _context.SaveChangesAsync();

        return user;

        // await Task.CompletedTask;
        // var index = _users.FindIndex(x => x.Id == tempUser.Id);
        // _users[index] = tempUser;
        // return tempUser;
    }

    public async Task<Unit> DeleteAsync(string id)
    {
        _context.Users.Remove(_context.Users.FirstOrDefault(x => x.Id == id));
        await _context.SaveChangesAsync();

        return Unit.Value;

        // await Task.CompletedTask;
        // var index = _users?.FindIndex(x => x.Id.ToString() == id) ?? -1;
        // if (index >= 0)
        // {
        //     _users.RemoveAt(index);

        //     return Unit.Value;
        // }
        // return Unit.Value;

    }

    public async Task<UserDTO> GetByEmailAsync(string email)
    {
        await Task.CompletedTask;

        var specification = new UserSpecification(email);
        return FindWithSpecificationPattern(specification).FirstOrDefault();

        // return _context.Users?.FirstOrDefault(x => x.Email == email)
        //        ?? null;

        // var index = _users?.FindIndex(x => x.Email == email) ?? -1;
        // if (index >= 0)
        // {
        //     var user = _users[index];
        //     return user;
        // }
        // return null;

    }

    public IEnumerable<UserDTO> FindWithSpecificationPattern(ISpecification<UserDTO> specification = null)
    {
        return SpecificationEvaluator<UserDTO>.GetQuery(_context.Users.AsQueryable(), specification);
    }
}