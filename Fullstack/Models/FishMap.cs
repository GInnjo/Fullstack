using Fullstack.Models.Items;
using Fullstack.Models.Static;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace Fullstack.Models;

public class FishMap
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [ReadOnly(true)]
    public ObjectId Id { get; set; }
    [BsonIgnore]
    public Stack<Fish>[,] fishArray { get; set; } = new Stack<Fish>[100, 100];
    [BsonIgnore]
    public string letters { get; set; } = "psctbywage";
    [BsonIgnore]
    public int fishCount { get; set; } = 10;

    public string [,] fishes { get; set; }

    public FishMap()
    {
        fishes = new string[100, 100];
        InitFishMap();
    }

    public Fish TakeFish(int x, int y)
    {
        if (fishArray[x, y] != null)
        {
            return fishArray[x, y].Pop();
        }
        else
        {
            throw new ArgumentException("No fish to take at specified location.");
        }
    }

    public void InitFishMap()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                fishes[i, j] = Helpers.GenerateRandomString(letters, fishCount);
            }
        }
    }

    public void PopulateFishArray()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < 100; j++)
            {
                foreach (char fishType in fishes[i, j])
                {
                    AddFish(i, j, fishType);
                }
            }
        }
    }

    public void AddFish(int x, int y, char fishType)
    {
        if (fishArray[x, y] == null)
        {
            fishArray[x, y] = new Stack<Fish>();
        }

        Fish fish = fishType switch
        {
            'p' => new Pike(),
            's' => new Salmon(),
            't' => new Trout(),
            'c' => new Catfish(),
            'b' => new Burbot(),
            'y' => new YellowPerch(),
            'w' => new Whitefish(),
            'a' => new Walleye(),
            'g' => new Sturgeon(),
            'e' => new Bass(),
            _ => throw new ArgumentException("Invalid fish type specified.")
        };

        fishArray[x, y].Push(fish);
    }
}
