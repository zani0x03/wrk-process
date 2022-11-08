using MongoDB.Bson;
using MongoDB.Driver;
using wrk_process.Models;
using wrk_process.Repositories.Interfaces;

namespace wrk_process.Repositories;

public class OrderRepository:IOrderRepository
{
    private readonly IMongoDBConn _mongoDBConn;
    private readonly MongoClient _mongoConnection;

    private readonly String _collectionName = "order";


    public OrderRepository(IMongoDBConn mongoDBConn){
        this._mongoDBConn = mongoDBConn;
        _mongoConnection = _mongoDBConn.RetMongoDBClient();
    }

    public async Task<Order> ReturnOrder(string orderId){
        var collection = (_mongoConnection.GetDatabase("Cluster0")).GetCollection<Order>(_collectionName);        
        var filter = Builders<Order>.Filter.Eq("_id",ObjectId.Parse(orderId));   
        return await collection.Find(filter).FirstOrDefaultAsync();
    }
}
