using MongoDB.Bson;
using Fullstack.Models.Static;

namespace Fullstack.Models
{
    public class Password
    {
        public ObjectId Id { get; set; }

        public byte[] hashedPassword;
        public byte[] salt;

        public Password(ObjectId id, string plainPassword) 
        { 
            this.Id = id;
            this.salt = Hasher.GenerateSalt();
            this.hashedPassword = Hasher.ComputeHash(plainPassword, this.salt);
        }

        public bool VerifyPassword(string plainPassword)
        {
            return Hasher.VerifyHash(plainPassword, this.hashedPassword, this.salt);
        }
    }
}
