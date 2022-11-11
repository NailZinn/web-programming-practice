using Microsoft.Extensions.Caching.Memory;

namespace Local_server.Sessions
{
    internal static class SessionManager
    {
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public static void CreateSession(object key, Func<Session> createSession)
        {
            var session = createSession();
            _cache.Set(key, session, 
                new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(2)));
        }

        public static bool CheckSession(object key)
        {
            return _cache.TryGetValue(key, out _);
        }

        public static Session? GetInfo(object key)
        {
            return _cache.TryGetValue<Session>(key, out var res)
                ? res
                : throw new ArgumentException($"could not find a session with key `{key}`");
;        }
    }
}
