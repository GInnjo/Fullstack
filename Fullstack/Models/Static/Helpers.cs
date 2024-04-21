using System.Reflection;
using System.Text;

namespace Fullstack.Models.Static;

static public class Helpers
{
    // Generates decimal number between two double values and cuts the double to 3 decimal places
    static public decimal GetRandomDecimal(double from, double to)
    {
        Random random = new Random();

        double random_double = random.NextDouble() * (to - 1) + from;
        string double_string = string.Format("{0:0.000}", random_double);
        return decimal.Parse(double_string);
    }

    // Generates string based on the given letters in a string and length 
    //(... if you apply same letter to the string it will have higher chance to appear...)
    public static string GenerateRandomString(string letters, int length)
    {
        Random random = new Random();
        StringBuilder builder = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(0, letters.Length);
            builder.Append(letters[index]);
        }

        return builder.ToString();
    }
}
