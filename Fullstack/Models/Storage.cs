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

    public ObjectId PlayerId { get; set; }

    public List<Item> Inventory { get; set; }
    public List<Item> Bank { get; set; }

    public Storage() 
    { 
        Inventory = new List<Item>();
        Bank = new List<Item>();
    }
}
