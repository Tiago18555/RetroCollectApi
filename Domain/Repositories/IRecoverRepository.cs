using MongoDB.Bson;
using MongoDB.Driver;

namespace Domain.Repositories;

public interface IRecoverRepository
{
    BsonDocument InsertDocument(string collectionName, BsonDocument document);
    BsonDocument UpdateDocument<TValue>(string collectionName, string idFieldName, TValue idValue, string fieldNameToUpdate, BsonValue newValue);
    int CountFailedAttemptsSinceLastSuccess(string username);
    bool Any(string collectionName, FilterDefinition<BsonDocument> filter);
    Task<BsonDocument> FindDocument<T>(string collectionName, string fieldName, T value, CancellationToken cts);
    void DeleteDocument(string collectionName, string fieldName, string value);
}