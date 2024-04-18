using System.Reflection;

namespace Fullstack.Models.Static;

static public class Helpers
{
    static public decimal GetRandomDecimal(double from, double to)
    {
        Random random = new Random();

        double random_double = random.NextDouble() * (to - 1) + from;
        string double_string = string.Format("{0:0.000}", random_double);
        return decimal.Parse(double_string);
    }
}
