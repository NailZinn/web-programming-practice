using Microsoft.Extensions.Caching.Memory;

namespace Local_server.Sessions
{
    internal class SessionManager
    {
        private readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public void CreateSession(object key, Func<Session> createSssion)
        {
            var session = createSssion();
            _cache.Set(key, session, 
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2)));
        }

        public bool CheckSession(object key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public Session? GetInfo(object key)
        {
            return _cache.TryGetValue<Session>(key, out var res)
                ? res
                : throw new ArgumentException($"could not find a session with key `{key}`");
;        }
    }
}
