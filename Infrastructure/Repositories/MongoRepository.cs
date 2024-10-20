using Domain.Repositories;
using Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class MongoRepository: IRecoverRepository
{
    private readonly MongoDBContext _context;

    public MongoRepository(MongoDBContext context)
    {
        _context = context;
    }

    public int CountFailedAttemptsSinceLastSuccess(string username)
    {
        var collection = _context.GetCollection<BsonDocument>("RecoverCollection");

        var successFilter = Builders<BsonDocument>.Filter.Eq("Username", username) & Builders<BsonDocument>.Filter.Eq("Success", true);
        var lastSuccess = collection.Find(successFilter).Sort(Builders<BsonDocument>.Sort.Descending("TimeStampHash")).FirstOrDefault();

        var failedFilter = Builders<BsonDocument>.Filter.Eq("Username", username) & Builders<BsonDocument>.Filter.Eq("Success", false);
        if (lastSuccess != null)
        {
            var timestampString = lastSuccess["TimeStampHash"].ToString().Split('-')[0];

            if (long.TryParse(timestampString, out var timestamp))
            {
                var lastSuccessTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).UtcDateTime;
                failedFilter &= Builders<BsonDocument>.Filter.Gt("TimeStampHash", lastSuccessTime);
            }
        }

        return (int)collection.CountDocuments(failedFilter);
    }

    public BsonDocument InsertDocument(string collectionName, BsonDocument document)
    {
        var collection = _context.GetCollection<BsonDocument>(collectionName);
        collection.InsertOne(document);
        return document;
    }

    public BsonDocument UpdateDocument<TValue>(string collectionName, string idFieldName, TValue idValue, string fieldNameToUpdate, BsonValue newValue)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(idFieldName, BsonValue.Create(idValue));
        var update = Builders<BsonDocument>.Update.Set(fieldNameToUpdate, newValue);
        var collection = _context.GetCollection<BsonDocument>(collectionName);
        return collection.FindOneAndUpdate(filter, update);            
    }

    public bool Any(string collectionName, FilterDefinition<BsonDocument> filter)
    {
        var collection = _context.GetCollection<BsonDocument>(collectionName);
        var result = collection.Find(filter).Any();
        return result;
    }

    public async Task<BsonDocument> FindDocument<T>(string collectionName, string fieldName, T value, CancellationToken cts)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(fieldName, value);
        var collection = _context.GetCollection<BsonDocument>(collectionName);
        var res = await collection.FindAsync(filter, null, cts);
        return await res.FirstOrDefaultAsync(cts);
    }

    public void DeleteDocument(string collectionName, string fieldName, string value)
    {
        var filter = Builders<BsonDocument>.Filter.Eq(fieldName, value);
        var collection = _context.GetCollection<BsonDocument>(collectionName);
        collection.DeleteOne(filter);
    }
}
