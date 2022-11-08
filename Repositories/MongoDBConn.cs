using wrk_process.Repositories.Interfaces;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;

namespace wrk_process.Repositories;

public class MongoDBConn:IMongoDBConn
{
    private MongoClient _mongoClient = null;

    public MongoDBConn(){
        if (_mongoClient == null){
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
            var settings = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MONGODB"));
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            _mongoClient = new MongoClient(settings);
        }
    }

    public MongoClient RetMongoDBClient (){
        return _mongoClient;
    }
}