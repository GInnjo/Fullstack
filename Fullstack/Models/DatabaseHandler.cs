using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Fullstack.Models {
    public class DatabaseHandler
    {
        public static IMongoDatabase? database;

        public static void Init(IConfiguration config, bool dev)
        {
            var connectionString = dev switch
            {
               true => config.GetConnectionString("MongoDBdev"),
               false => config.GetConnectionString("MongoDB")
            };
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
                    // Extract the id, if set string in Mongo DB, from the error message
                    string pattern1 = @"_id: ""([^""]+)""";
                    // Extract the id, if set ObjectId in Mongo DB, from the error message
					string pattern2 = @"_id:\s*ObjectId\('([^']+)'\)";
					Match match1 = Regex.Match(ex.Message, pattern1);
					Match match2 = Regex.Match(ex.Message, pattern2);
					string id = match1.Groups[1].Value;
                    ObjectId oid = ObjectId.Parse(match2.Groups[1].Value);


                    var filter = id switch
                    {
						"" => Builders<T>.Filter.Eq("_id", oid),
						_ => Builders<T>.Filter.Eq("_id", id)
					};

                    collection.ReplaceOne(filter, record);
                    Console.WriteLine($"Data with id: {id}{oid} has been replaced.");

                }
            }
            return record;
        }
        public static void Delete<T>(ObjectId id)
        {
            if(typeof(T) == typeof(User))
            {
                User user = GetById<User>(id);
                Delete<Password>(user.Id);
                Delete<Storage>(user.Id);
                Delete<GameInstance>(user.Id);
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

		public static User GetUserByUsername(string username)
		{
			var collection = database?.GetCollection<User>("User");
			var filter = Builders<User>.Filter.Eq("Username", username);
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