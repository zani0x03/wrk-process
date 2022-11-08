using MongoDB.Driver;

namespace wrk_process.Repositories.Interfaces;

public interface IMongoDBConn{
    MongoClient RetMongoDBClient ();
}