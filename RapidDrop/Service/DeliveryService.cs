using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RapidDrop.Service;

public class DeliveryService
{
    private readonly IMongoCollection<Delivery> _deliveries;

    public DeliveryService(IOptions<MongoDBSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _deliveries = database.GetCollection<Delivery>(settings.Value.CollectionName);
    }

    public async Task<List<Delivery>> GetAllAsync() =>
        await _deliveries.Find(delivery => true).ToListAsync();

    public async Task<Delivery> GetByIdAsync(string id) =>
        await _deliveries.Find(delivery => delivery.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Delivery delivery) =>
        await _deliveries.InsertOneAsync(delivery);

    public async Task UpdateAsync(string id, Delivery delivery) =>
        await _deliveries.ReplaceOneAsync(d => d.Id == id, delivery);

    public async Task DeleteAsync(string id) =>
        await _deliveries.DeleteOneAsync(d => d.Id == id);
}
