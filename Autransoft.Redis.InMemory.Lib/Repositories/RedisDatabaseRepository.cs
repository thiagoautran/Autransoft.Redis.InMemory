using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Models;

namespace Name
{
    public class RedisDatabaseRepository : IRedisDatabase
    {
        private static Dictionary<string, object> _databaseInMemory;

        public static Dictionary<string, object> DatabaseInMemory
        {
            get
            {
                if(_databaseInMemory == null)
                    _databaseInMemory = new Dictionary<string, object>();

                return _databaseInMemory;
            }
            set
            {
                _databaseInMemory = value;
            }
        }

        public static void Clean() => _databaseInMemory = null;

        public IDatabase Database => throw new NotImplementedException();

        public ISerializer Serializer => throw new NotImplementedException();

        public async Task<bool> AddAllAsync<T>(IList<Tuple<string, T>> items, DateTimeOffset expiresAt, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(items == null || items.Count == 0)
                return false;

            foreach(var item in items)
                _databaseInMemory.Add(item.Item1, item.Item2);

            return true;
        }

        public async Task<bool> AddAllAsync<T>(IList<Tuple<string, T>> items, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(items == null || items.Count == 0)
                return false;

            foreach(var item in items)
                _databaseInMemory.Add(item.Item1, item.Item2);

            return true;
        }

        public async Task<bool> AddAllAsync<T>(IList<Tuple<string, T>> items, TimeSpan expiresIn, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            if(items == null || items.Count == 0)
                return false;

            foreach(var item in items)
                _databaseInMemory.Add(item.Item1, item.Item2);

            return true;
        }

        public async Task<bool> AddAsync<T>(string key, T value, When when = When.Always, CommandFlags flag = CommandFlags.None, HashSet<string> tags = null)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            _databaseInMemory.Add(key, value);

            return true;
        }

        public async Task<bool> AddAsync<T>(string key, T value, DateTimeOffset expiresAt, When when = When.Always, CommandFlags flag = CommandFlags.None, HashSet<string> tags = null)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            _databaseInMemory.Add(key, value);

            return true;
        }

        public async Task<bool> AddAsync<T>(string key, T value, TimeSpan expiresIn, When when = When.Always, CommandFlags flag = CommandFlags.None, HashSet<string> tags = null)
        {
            if(string.IsNullOrEmpty(key) || value == null)
                return false;

            _databaseInMemory.Add(key, value);

            return true;
        }

        public async Task<bool> ExistsAsync(string key, CommandFlags flag = CommandFlags.None)
        {
            if(_databaseInMemory.Keys == null || _databaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return false;

            return _databaseInMemory.Keys.Any(keyInMemory => keyInMemory.Trim().ToUpper() == key.Trim().ToUpper());
        }

        public async Task FlushDbAsync() => _databaseInMemory = new Dictionary<string, object>();

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys)
        {
            var listT = new Dictionary<string, T>();

            if(_databaseInMemory == null)
                return new Dictionary<string, T>();

            foreach(var key in keys.Where(key => _databaseInMemory.Keys.Any(keyInMemory => keyInMemory == key)))
            {
                if(_databaseInMemory[key] != null)
                {
                    var value = (T)_databaseInMemory[key];

                    if(value != null)
                        listT.Add(key, (T)_databaseInMemory[key]);
                }
            }

            return listT;
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, DateTimeOffset expiresAt)
        {
            var listT = new Dictionary<string, T>();

            if(_databaseInMemory == null)
                return new Dictionary<string, T>();

            foreach(var key in keys.Where(key => _databaseInMemory.Keys.Any(keyInMemory => keyInMemory == key)))
            {
                if(_databaseInMemory[key] != null)
                {
                    var value = (T)_databaseInMemory[key];

                    if(value != null)
                        listT.Add(key, (T)_databaseInMemory[key]);
                }
            }

            return listT;
        }

        public async Task<IDictionary<string, T>> GetAllAsync<T>(IEnumerable<string> keys, TimeSpan expiresIn)
        {
            if(_databaseInMemory == null || _databaseInMemory.Count == 0 || _databaseInMemory.Keys.Count == 0)
                return new Dictionary<string, T>();

            var listT = new Dictionary<string, T>();

            foreach(var key in keys.Where(key => _databaseInMemory.Keys.Any(keyInMemory => keyInMemory == key)))
            {
                if(_databaseInMemory[key] != null)
                {
                    var value = (T)_databaseInMemory[key];

                    if(value != null)
                        listT.Add(key, (T)_databaseInMemory[key]);
                }
            }

            return listT;
        }

        public async Task<T> GetAsync<T>(string key, CommandFlags flag = CommandFlags.None)
        {
            if(_databaseInMemory == null || _databaseInMemory.Count == 0 || _databaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return default(T);

            var keyMemory = _databaseInMemory.Keys.FirstOrDefault(keyInMemory => keyInMemory == key);
            if(_databaseInMemory[key] == null) 
                return default(T);

            return (T)_databaseInMemory[key];
        }

        public async Task<T> GetAsync<T>(string key, DateTimeOffset expiresAt, CommandFlags flag = CommandFlags.None)
        {
            if(_databaseInMemory == null || _databaseInMemory.Count == 0 || _databaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return default(T);

            var keyMemory = _databaseInMemory.Keys.FirstOrDefault(keyInMemory => keyInMemory == key);
            if(_databaseInMemory[key] == null) 
                return default(T);

            return (T)_databaseInMemory[key];
        }

        public async Task<T> GetAsync<T>(string key, TimeSpan expiresIn, CommandFlags flag = CommandFlags.None)
        {
            if(_databaseInMemory == null || _databaseInMemory.Count == 0 || _databaseInMemory.Keys.Count == 0 || string.IsNullOrEmpty(key))
                return default(T);

            var keyMemory = _databaseInMemory.Keys.FirstOrDefault(keyInMemory => keyInMemory == key);
            if(_databaseInMemory[key] == null) 
                return default(T);

            return (T)_databaseInMemory[key];
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

            if(_databaseInMemory == null || _databaseInMemory.Count == 0 || _databaseInMemory.Keys.Count == 0)
                return default(long);

            foreach(var key in keys)
            {
                if(string.IsNullOrEmpty(key) || _databaseInMemory[key] == null || !_databaseInMemory.Keys.Any(keyInMemory => keyInMemory == key))
                    continue;

                _databaseInMemory.Remove(key);
            }

            return default(long);
        }

        public async Task<bool> RemoveAsync(string key, CommandFlags flag = CommandFlags.None)
        {
            if(string.IsNullOrEmpty(key))
                return false;

            if(_databaseInMemory == null || _databaseInMemory.Count == 0 || _databaseInMemory.Keys.Count == 0)
                return false;

            if(_databaseInMemory[key] == null || !_databaseInMemory.Keys.Any(keyInMemory => keyInMemory == key))
                return false;

            _databaseInMemory.Remove(key);

            return true;
        }

        public Task<long> RemoveByTagAsync(string tag, CommandFlags flags = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReplaceAsync<T>(string key, T value, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReplaceAsync<T>(string key, T value, DateTimeOffset expiresAt, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReplaceAsync<T>(string key, T value, TimeSpan expiresIn, When when = When.Always, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(SaveType saveType, CommandFlags flag = CommandFlags.None)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> SearchKeysAsync(string pattern)
        {
            throw new NotImplementedException();
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