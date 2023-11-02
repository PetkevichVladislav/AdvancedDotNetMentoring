using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CartingService.DataAcessLayer.Models
{
    public record Image
    {
        [BsonRepresentation(BsonType.String)]
        public string? Url { get; init; }

        public string? AlternativeText { get; init; }
    }
}
