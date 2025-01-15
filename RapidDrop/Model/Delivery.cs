using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Delivery
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string CustomerName { get; set; }
    public string WhatsAppNumber { get; set; }
    public string Brand { get; set; }
    public string Product { get; set; }
    public decimal Price { get; set; }
    public DateTime DeliveryDate { get; set; }
}
