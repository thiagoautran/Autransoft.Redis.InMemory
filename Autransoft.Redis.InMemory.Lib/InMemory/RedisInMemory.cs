using System;
using Autransoft.Redis.InMemory.Lib.Extensions;
using Autransoft.Redis.InMemory.Lib.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Autransoft.Redis.InMemory.Lib.InMemory
{
    public static class RedisInMemory
    {
        public static void AddToDependencyInjection(IServiceCollection serviceCollection)
        {
            serviceCollection.ReplaceSingleton(typeof(IRedisCacheClient), typeof(RedisCacheClientInMemory));
            serviceCollection.ReplaceSingleton(typeof(IRedisDatabase), typeof(RedisDatabaseRepository));
        }

        public static IRedisDatabase Get(IServiceProvider serviceProvider) => serviceProvider.GetService<IRedisDatabase>();

        public static void Clean() => RedisDatabaseRepository.Clean();
    }
}