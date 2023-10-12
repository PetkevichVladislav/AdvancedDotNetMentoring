using MongoDB.Bson.Serialization.Attributes;

namespace CartingService.DataAcessLayer.Models
{
    public record Cart
    {
        [BsonId]
        public int Id { get; init; }

        public List<LineItem> LineItems { get; init; } = new List<LineItem>();
    }
}
