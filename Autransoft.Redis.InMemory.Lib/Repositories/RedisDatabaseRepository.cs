using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autransoft.Redis.InMemory.Lib.Entities;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Models;

namespace Autransoft.Redis.InMemory.Lib.Repositories
{
    public class RedisDatabaseRepository : IRedisDatabase
    {
        private static Dictionary<string, ValueEntity> _databaseInMemory;

        public static Dictionary<string, ValueEntity> DatabaseInMemory
        {
            get
            {
                if(_databaseInMemory == null)
                    _databaseInMemory = new Dictionary<string, ValueEntity>();

                return _databaseInMemory;
            }
        }

        public Dictionary<string, ValueEntity> GetDatabase() => _databaseInMemory;

        public void SetDatabase(Dictionary<string, ValueEntity> databaseInMemory) => _databaseInMemory = databaseInMemory;

        public static void Clean() => _databaseInMemory = null;

        public IDatabase Database => throw new NotImplementedException();

        public ISerializer Serializer => throw new NotImplementedException();

        public async Task<bool> AddAllAsync<T>(IList<Tuple<string, T>> items, DateTimeOffset expiresAt, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(items == null || items.Count == 0)
                return false;

            foreach(var item in items)
                DatabaseInMemory.Add(item.Item1, new ValueEntity
                { 
                    Obj = item.Item2,
                    Tag = new HashSet<string>()
                });

            return true;
        }

        public async Task<bool> AddAllAsync<T>(IList<Tuple<string, T>> items, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(items == null || items.Count == 0)
                return false;

            foreach(var item in items)
                DatabaseInMemory.Add(item.Item1, new ValueEntity
                { 
                    Obj = item.Item2,
                    Tag = new HashSet<string>()
                });

            return true;
        }

        public async Task<bool> AddAllAsync<T>(IList<Tuple<string, T>> items, TimeSpan expiresIn, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(items == null || items.Count == 0)
                return false;

            foreach(var item in items)
                DatabaseInMemory.Add(item.Item1, new ValueEntity
                { 
                    Obj = item.Item2,
                    Tag = new HashSet<string>()
                });

            return true;
        }

        public async Task<bool> AddAsync<T>(string key, T value, When when = When.Always, CommandFlags flag = CommandFlags.None, HashSet<string> tags = null)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            DatabaseInMemory.Add(key, new ValueEntity
            { 
                Obj = value,
                Tag = tags
            });

            return true;
        }

        public async Task<bool> AddAsync<T>(string key, T value, DateTimeOffset expiresAt, When when = When.Always, CommandFlags flag = CommandFlags.None, HashSet<string> tags = null)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            DatabaseInMemory.Add(key, new ValueEntity
            { 
                Obj = value,
                Tag = tags
            });

            return true;
        }

        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn, When when = When.Always, CommandFlags flag = CommandFlags.None, HashSet<string> tags = null)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            DatabaseInMemory.Add(key, new ValueEntity
            { 
                Obj = value,
                Tag = tags
            });

            return true;
        }

        public async Task<bool> ExistsAsync(string key, CommandFlags flag = CommandFlags.None)
        {
            if(DatabaseInMemory.Keys == null || DatabaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return false;

            return DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory.Trim().ToUpper() == key.Trim().ToUpper());
        }

        public async Task FlushDbAsync() => _databaseInMemory = new Dictionary<string, ValueEntity>();

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            var listT = new Dictionary<string, T>();

            if(DatabaseInMemory == null)
                return new Dictionary<string, T>();

            foreach(var key in keys.Where(key => DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key)))
            {
                if(DatabaseInMemory[key] != null && DatabaseInMemory[key].Obj != null)
                {
                    var value = (T)DatabaseInMemory[key].Obj;

                    if(value != null)
                        listT.Add(key, (T)DatabaseInMemory[key].Obj);
                }
            }

            return listT;
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, DateTimeOffset expiresAt)
        {
            var listT = new Dictionary<string, T>();

            if(DatabaseInMemory == null)
                return new Dictionary<string, T>();

            foreach(var key in keys.Where(key => DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key)))
            {
                if(DatabaseInMemory[key] != null && DatabaseInMemory[key].Obj != null)
                {
                    var value = (T)DatabaseInMemory[key].Obj;

                    if(value != null)
                        listT.Add(key, (T)DatabaseInMemory[key].Obj);
                }
            }

            return listT;
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, TimeSpan expiresIn)
        {
            if(DatabaseInMemory == null || DatabaseInMemory.Count == 0 || DatabaseInMemory.Keys.Count == 0)
                return new Dictionary<string, T>();

            var listT = new Dictionary<string, T>();

            foreach(var key in keys.Where(key => DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key)))
            {
                if(DatabaseInMemory[key] != null && DatabaseInMemory[key].Obj != null)
                {
                    var value = (T)DatabaseInMemory[key].Obj;

                    if(value != null)
                        listT.Add(key, (T)DatabaseInMemory[key].Obj);
                }
            }

            return listT;
        }

        public async Task<T> GetAsync<T>(string key, CommandFlags flag = CommandFlags.None)
        {
            if(DatabaseInMemory == null || DatabaseInMemory.Count == 0 || DatabaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return default(T);

            var keyMemory = DatabaseInMemory.Keys.FirstOrDefault(keyInMemory => keyInMemory == key);
            if(DatabaseInMemory[key] == null && DatabaseInMemory[key].Obj == null) 
                return default(T);

            return (T)DatabaseInMemory[key].Obj;
        }

        public async Task<T> GetAsync<T>(string key, DateTimeOffset expiresAt, CommandFlags flag = CommandFlags.None)
        {
            if(DatabaseInMemory == null || DatabaseInMemory.Count == 0 || DatabaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return default(T);

            var keyMemory = DatabaseInMemory.Keys.FirstOrDefault(keyInMemory => keyInMemory == key);
            if(DatabaseInMemory[key] == null && DatabaseInMemory[key].Obj == null) 
                return default(T);

            return (T)DatabaseInMemory[key].Obj;
        }

        public async Task<T> GetAsync<T>(string key, TimeSpan expiresIn, CommandFlags flag = CommandFlags.None)
        {
            if(DatabaseInMemory == null || DatabaseInMemory.Count == 0 || DatabaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return default(T);

            var keyMemory = DatabaseInMemory.Keys.FirstOrDefault(keyInMemory => keyInMemory == key);
            if(DatabaseInMemory[key] == null && DatabaseInMemory[key].Obj == null) 
                return default(T);

            return (T)DatabaseInMemory[key].Obj;
        }

        public Task<IEnumerable<T>> GetByTagAsync<T>(string tag, CommandFlags commandFlags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, string>> GetInfoAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<InfoDetail>> GetInfoCategorizedAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashDeleteAsync(string hashKey, string key, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<long> HashDeleteAsync(string hashKey, IEnumerable<string> keys, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashExistsAsync(string hashKey, string key, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, T>> HashGetAllAsync<T>(string hashKey, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<T> HashGetAsync<T>(string hashKey, string key, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, T>> HashGetAsync<T>(string hashKey, IList<string> keys, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<long> HashIncerementByAsync(string hashKey, string key, long value, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<double> HashIncerementByAsync(string hashKey, string key, double value, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> HashKeysAsync(string hashKey, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<long> HashLengthAsync(string hashKey, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, T> HashScan<T>(string hashKey, string pattern, int pageSize = 10, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HashSetAsync<T>(string hashKey, string key, T value, bool nx = false, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task HashSetAsync<T>(string hashKey, IDictionary<string, T> values, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> HashValuesAsync<T>(string hashKey, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<long> ListAddToLeftAsync<T>(string key, T item, When when = When.Always, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<long> ListAddToLeftAsync<T>(string key, T[] items, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> ListGetFromRightAsync<T>(string key, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<long> PublishAsync<T>(RedisChannel channel, T message, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public async Task<long> RemoveAllAsync(IEnumerable<string> keys, CommandFlags flag = CommandFlags.None)
        {
            if(keys == null || keys.Count() == 0)
                return default(long);

            if(DatabaseInMemory == null || DatabaseInMemory.Count == 0 || DatabaseInMemory.Keys.Count == 0)
                return default(long);

            foreach(var key in keys)
            {
                if(string.IsNullOrEmpty(key) || DatabaseInMemory[key] == null || !DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key))
                    continue;

                DatabaseInMemory.Remove(key);
            }

            return default(long);
        }

        public async Task<bool> RemoveAsync(string key, CommandFlags flag = CommandFlags.None)
        {
            if(string.IsNullOrEmpty(key))
                return false;

            if(DatabaseInMemory == null || DatabaseInMemory.Count == 0 || DatabaseInMemory.Keys.Count == 0)
                return false;

            if(DatabaseInMemory[key] == null || !DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key))
                return false;

            DatabaseInMemory.Remove(key);

            return true;
        }

        public Task<long> RemoveByTagAsync(string tag, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ReplaceAsync<T>(string key, T value, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            if(DatabaseInMemory == null || DatabaseInMemory.Count() == 0 || DatabaseInMemory.Keys.Count() == 0 || !DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key))
                return false;

            DatabaseInMemory[key].Obj = value;

            return true;
        }

        public async Task<bool> ReplaceAsync<T>(string key, T value, DateTimeOffset expiresAt, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            if(DatabaseInMemory == null || DatabaseInMemory.Count() == 0 || DatabaseInMemory.Keys.Count() == 0 || !DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key))
                return false;

            DatabaseInMemory[key].Obj = value;

            return true;
        }

        public async Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresIn, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            if(DatabaseInMemory == null || DatabaseInMemory.Count() == 0 || DatabaseInMemory.Keys.Count() == 0 || !DatabaseInMemory.Keys.Any(keyInMemory => keyInMemory == key))
                return false;

            DatabaseInMemory[key].Obj = value;

            return true;
        }

        public Task SaveAsync(SaveType saveType, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> SearchKeysAsync(string pattern)
        {
            if(string.IsNullOrEmpty(pattern))
                return default(IEnumerable<string>);

            if(DatabaseInMemory == null || DatabaseInMemory.Count() == 0 || DatabaseInMemory.Keys.Count() == 0)
                return default(IEnumerable<string>);

            return DatabaseInMemory.Keys.Where
            (
                keyInMemory => 
                {
                    var exist = true;

                    foreach(var caracter in pattern.Split('*'))
                        if(exist && !keyInMemory.Contains(caracter))
                            exist = false;

                    return exist;
                }
            )
            .ToList();
        }

        public Task<long> SetAddAllAsync<T>(string key, CommandFlags flag = CommandFlags.None, params T[] items) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAddAsync<T>(string key, T item, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetContainsAsync<T>(string key, T item, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<string[]> SetMemberAsync(string memberName, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> SetMembersAsync<T>(string key, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<T> SetPopAsync<T>(string key, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> SetPopAsync<T>(string key, long count, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<long> SetRemoveAllAsync<T>(string key, CommandFlags flag = CommandFlags.None, params T[] items) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetRemoveAsync<T>(string key, T item, CommandFlags flag = CommandFlags.None) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> SortedSetAddAsync<T>(string key, T value, double score, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<double> SortedSetAddIncrementAsync<T>(string key, T value, double score, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> SortedSetRangeByScoreAsync<T>(string key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SortedSetRemoveAsync<T>(string key, T value, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync<T>(RedisChannel channel, Func<T, Task> handler, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeAllAsync(CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task UnsubscribeAsync<T>(RedisChannel channel, Func<T, Task> handler, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, DateTimeOffset expiresAt, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<IDictionary<string, bool>> UpdateExpiryAllAsync(string[] keys, TimeSpan expiresIn, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateExpiryAsync(string key, DateTimeOffset expiresAt, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateExpiryAsync(string key, TimeSpan expiresIn, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }
    }
}