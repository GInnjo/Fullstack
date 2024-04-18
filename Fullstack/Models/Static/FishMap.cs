using Fullstack.Models.Items;

namespace Fullstack.Models.Static;

static public class FishMap
{

    static int rows = 100; 
    static int columns = 100; 

    static public Stack<Fish>[,] fishArray = new Stack<Fish>[rows, columns];


    static public void TakeFish(int x, int y)
    {
        if (fishArray[x, y] != null)
        {
            fishArray[x, y].Pop();
        }
        else 
        { 
            throw new ArgumentException("No fish to take at specified location.");
        }
    }

    static public void AddFish(int x, int y, char fishType)
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
