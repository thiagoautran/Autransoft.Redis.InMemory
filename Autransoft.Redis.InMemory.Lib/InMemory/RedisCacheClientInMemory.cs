using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Autransoft.Redis.InMemory.Lib.InMemory
{
    public class RedisCacheClientInMemory : IRedisCacheClient
    {
        private readonly IRedisDatabase _redisDatabase;

        public RedisCacheClientInMemory(IRedisDatabase redisDatabase)
        {
            _redisDatabase = redisDatabase;
        }

        public IRedisDatabase Db0 => _redisDatabase;

        public IRedisDatabase Db1 => _redisDatabase;

        public IRedisDatabase Db2 => _redisDatabase;

        public IRedisDatabase Db3 => _redisDatabase;

        public IRedisDatabase Db4 => _redisDatabase;

        public IRedisDatabase Db5 => _redisDatabase;

        public IRedisDatabase Db6 => _redisDatabase;

        public IRedisDatabase Db7 => _redisDatabase;

        public IRedisDatabase Db8 => _redisDatabase;

        public IRedisDatabase Db9 => _redisDatabase;

        public IRedisDatabase Db10 => _redisDatabase;

        public IRedisDatabase Db11 => _redisDatabase;

        public IRedisDatabase Db12 => _redisDatabase;

        public IRedisDatabase Db13 => _redisDatabase;

        public IRedisDatabase Db14 => _redisDatabase;

        public IRedisDatabase Db15 => _redisDatabase;

        public IRedisDatabase Db16 => _redisDatabase;

        public ISerializer Serializer => throw new System.NotImplementedException();

        public IRedisDatabase GetDb(int dbNumber, string keyPrefix = null) => _redisDatabase;

        public IRedisDatabase GetDbFromConfiguration() => _redisDatabase;
    }
}