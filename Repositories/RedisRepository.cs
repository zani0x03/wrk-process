using StackExchange.Redis;
using wrk_process.Repositories.Interfaces;

namespace wrk_process.Repositories;


public class RedisRepository:IRedisRepository{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _redisDatabase;

    public RedisRepository(IConnectionMultiplexer redis){
        _redis = redis;
        _redisDatabase = _redis.GetDatabase();
    }


    public async Task SetStringData(string key, string data){
        await _redisDatabase.StringSetAsync(key,data);
    }

    public async Task<string> GetStringData (string key){

        return await _redisDatabase.StringGetAsync(key);
    }


}