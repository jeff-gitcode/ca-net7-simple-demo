using Application.Interface.SPI;

using Ardalis.GuardClauses;

using Domain;

using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Services
{
    public class MemoryCacheUsersService : ICacheUsersService
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheUsersService(IMemoryCache cache)
        {
            Guard.Against.Null(cache, nameof(cache));

            _cache = cache;
        }

        public async Task<IEnumerable<UserDTO>> Get(string key)
        {
            await Task.CompletedTask;
            return _cache.Get<IEnumerable<UserDTO>>(key);
        }

        public async Task Remove(string key)
        {
            await Task.CompletedTask;
            _cache.Remove(key);
        }

        public async Task Set(string key, IEnumerable<UserDTO> value)
        {
            await Task.CompletedTask;

            var item = _cache.Get(key);

            if (item == null)
            {
                var cacheOption = new MemoryCacheEntryOptions()
                            .SetAbsoluteExpiration(TimeSpan.FromMinutes(0.5));

                _cache.Set(key, value, cacheOption);
            }
        }

        // public Task<IEnumerable<UserDTO>> Get(string key)
        // {
        //     throw new NotImplementedException();
        // }

        // public Task Remove(string key)
        // {
        //     throw new NotImplementedException();
        // }

        // public Task Set(string key, IEnumerable<UserDTO> value)
        // {
        //     throw new NotImplementedException();
        // }

        // public void Set(string key, T value)
        // {
        //     var item = _cache.Get(key);

        //     if (item == null)
        //     {
        //         var cacheOption = new MemoryCacheEntryOptions()
        //                     .SetAbsoluteExpiration(TimeSpan.FromMinutes(0.5));

        //         _cache.Set(key, value, cacheOption);
        //     }
        // }

        // public T Get(string key)
        // {
        //     return _cache.Get<T>(key);
        // }

        // public void Remove(string key)
        // {
        //     _cache.Remove(key);
        // }

        // public bool Equals(MemoryCacheUsersService other)
        // {
        //     throw new NotImplementedException();
        // }

        // public Task Set(string key, IEnumerable<UserDTO> value)
        // {
        //     throw new NotImplementedException();
        // }

        // Task<IEnumerable<UserDTO>> ICacheUsersService.Get(string key)
        // {
        //     throw new NotImplementedException();
        // }

        // Task ICacheUsersService.Remove(string key)
        // {
        //     throw new NotImplementedException();
        // }
    }
}