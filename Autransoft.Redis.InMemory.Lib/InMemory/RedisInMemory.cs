using System;
using Autransoft.Redis.InMemory.Lib.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Autransoft.Redis.InMemory.Lib.InMemory
{
    public static class RedisInMemory
    {
        public static void AddToDependencyInjection(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRedisCacheClient, RedisCacheClientInMemory>();
            serviceCollection.AddSingleton<IRedisDatabase, RedisDatabaseRepository>();
        }

        public static IRedisDatabase Get(IServiceProvider serviceProvider) => serviceProvider.GetRequiredService<IRedisDatabase>();

        public static void Clean() => RedisDatabaseRepository.Clean();
    }
}