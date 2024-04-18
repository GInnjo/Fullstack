using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Fullstack.Models {
    public class DatabaseHandler
    {
        public static IMongoDatabase? database;

        public static void Init(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("MongoDB");
            var client = new MongoClient(connectionString);
            database = client.GetDatabase("Big");
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
                    //Ööö.. mongo exceptionissa on ilmotus duplicate id:stä muodossa...
                    //...bla bla bla... { _id: "(joku id)" } ni regexillä parsee sen (joku id) osion ja siinä on id.
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

        public static T GetByObjectID<T>(ObjectId id) 
        {
            var collection = database.GetCollection<T>(typeof(T).Name);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = collection.Find(filter).FirstOrDefault();
            return result;
        }

        public static List<T> GetAll<T>(string table) 
        {
            var collection = database.GetCollection<T>(table);
            return collection.Find(new BsonDocument()).ToList();
        }
    }
}