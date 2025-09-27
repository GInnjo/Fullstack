using System.Collections.Concurrent;
using System.Security.Cryptography;

public class TokenStorage
{
    private readonly ConcurrentDictionary<string, TokenEntry> _tokens = new();
    private readonly TimeSpan _tokenTimeout = TimeSpan.FromMinutes(30);

    private class TokenEntry
    {
        public string UserId { get; init; }
        public DateTime LastAccess { get; set; }
    }

    public string GenerateToken(string userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        _tokens[token] = new TokenEntry
        {
            UserId = userId,
            LastAccess = DateTime.UtcNow
        };
        return token;
    }

    public bool ValidateToken(string token, out string userId)
    {
        userId = null;

        if (_tokens.TryGetValue(token, out var entry))
        {
            entry.LastAccess = DateTime.UtcNow;
            userId = entry.UserId;
            Console.WriteLine("Returning true");
            return true;
        }

        return false;
    }

    public void CleanupExpiredTokens()
    {
        foreach (var kvp in _tokens)
        {
            if (DateTime.UtcNow - kvp.Value.LastAccess >= _tokenTimeout)
                _tokens.TryRemove(kvp.Key, out _);
        }
    }
}