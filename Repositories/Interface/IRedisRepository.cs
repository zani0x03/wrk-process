namespace wrk_process.Repositories.Interfaces;

public interface IRedisRepository{
    Task<string> GetStringData (string key);   
    Task SetStringData(string key, string data);
}