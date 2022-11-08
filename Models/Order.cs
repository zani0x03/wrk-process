using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace wrk_process.Models;


public class Order{
    [BsonId]
    public ObjectId Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ClientId { get; set; }
    public DateTime? InitialDate  { get; set; }
    public List<ProductOrder> Products { get; set; }
}