using MongoDB.Bson;
using MongoDB.Driver;

namespace Domain.Repositories;

public interface IRecoverRepository
{
    BsonDocument InsertDocument(string collectionName, BsonDocument document);
    BsonDocument UpdateDocument<TValue>(string collectionName, string idFieldName, TValue idValue, string fieldNameToUpdate, BsonValue newValue);
    int CountFailedAttemptsSinceLastSuccess(Guid userId);
    bool Any(string collectionName, FilterDefinition<BsonDocument> filter);
    BsonDocument FindDocument<T>(string collectionName, string fieldName, T value);
    void DeleteDocument(string collectionName, string fieldName, string value);
}