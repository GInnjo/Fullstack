using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Fullstack.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [ReadOnly(true)]
    public ObjectId Id { get; set; }
    public string? Role { get; set; }
    [Required]
    public string? FirstName { get; set; }
    [Required]
    public string? LastName { get; set; }
    [Required]
    public string? Email { get; set; }
    [Required]
    public string? Username { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastWebLogin { get; set; }
	public DateTime? LastGameLogin { get; set; }
	public ObjectId PasswordId { get; set; }
    public ObjectId StorageId { get; set; }
    public User() { }
}
