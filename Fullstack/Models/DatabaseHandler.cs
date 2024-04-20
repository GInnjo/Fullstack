using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Fullstack.Models {
    public class DatabaseHandler
    {
        public static IMongoDatabase? database;

        public static void Init(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            database = client.GetDatabase("Fullstack");
        }

        public static T Save<T>(T record)
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            try
            {
                collection.InsertOne(record);
            }
            catch (MongoWriteException ex)
            {
                if (ex.Message.Contains("duplicate key error"))
                {
                    string pattern = @"_id: ""([^""]+)""";
                    Match match = Regex.Match(ex.Message, pattern);
                    string id = match.Groups[1].Value;


                    var filter = Builders<T>.Filter.Eq("_id", id);
                    collection.ReplaceOne(filter, record);
                    Console.WriteLine($"Data with id: {id} has been replaced.");

                }
            }
            return record;
        }
        public static void Delete<T>(ObjectId id)
        {
            if(typeof(T) == typeof(User))
            {
                User user = GetById<User>(id);
                Delete<Password>(user.PasswordId);
                Delete<Storage>(user.StorageId);
            }

            var collection = database.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var objectToDelete = collection.Find(filter).FirstOrDefault();

            collection.DeleteOne(filter);
        }


        public static T GetById<T>(ObjectId id) 
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = collection.Find(filter).FirstOrDefault();
            return result;
        }

        public static User GetUserByEmail(string email)
        {
            var collection = database?.GetCollection<User>("User");
            var filter = Builders<User>.Filter.Eq("Email", email);
            var result = collection?.Find(filter).FirstOrDefault();
            return result;
        }

        public static List<T> GetAll<T>(string table) 
        {
            var collection = database.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
    }
}