using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace Fullstack.Models;

public class GameInstance
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [ReadOnly(true)]
    public ObjectId Id { get; set; }
    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? LastUpTime { get; set; }
    public string? Status { get; set; }

    public ObjectId? OwnerPlayerId { get; set; }
    public List<ObjectId> InvitedPlayerIds { get; set; }

    public GameInstance() 
    { 
        InvitedPlayerIds = new List<ObjectId>();
    }
}
