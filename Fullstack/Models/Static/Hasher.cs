using System.Security.Cryptography;
using System.Text;

namespace Fullstack.Models.Static;

public class Hasher
{
    public static byte[] GenerateSalt()
    {
        byte[] salt = new byte[16];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public static byte[] ComputeHash(string password, byte[] salt)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] combinedBytes = Encoding.UTF8.GetBytes(password + Convert.ToBase64String(salt));

            return sha256.ComputeHash(combinedBytes);
        }
    }

    public static bool VerifyHash(string password, byte[] hash, byte[] salt)
    {
        if (hash == ComputeHash(password, salt))
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

}
