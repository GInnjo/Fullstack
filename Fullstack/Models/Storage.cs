using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;

namespace Fullstack.Models;

public class Storage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [ReadOnly(true)]
    public ObjectId Id { get; set; }
    public List<Item> Inventory { get; set; }
    public List<Item> Bank { get; set; }

    public Storage(ObjectId id) 
    { 
        this.Id = id;
        Inventory = new List<Item>();
        Bank = new List<Item>();
    }
}
