using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Security.Claims;

public class GameHub : Hub
{
    private static ConcurrentDictionary<string, (string UserId, string ClientType)> connections = new();

    private readonly TokenStorage _tokenStorage;

    public GameHub(TokenStorage tokenStorage)
    {
        _tokenStorage = tokenStorage;
    }

    public class TokenValidationResult
    {
        public bool Success { get; set; }
        public string UserId { get; set; }
    }

    public Task<string> RequestFish()
    {
        return Task.FromResult("Pike");
    }

    public Task<TokenValidationResult> ValidateToken(string token)
    {
        if (_tokenStorage.ValidateToken(token, out var userId))
        {
            Console.WriteLine("Token accepted.");
            return Task.FromResult(new TokenValidationResult
            {
                Success = true,
                UserId = userId
            });
        }

        Console.WriteLine("Token denied.");
        return Task.FromResult(new TokenValidationResult
        {
            Success = false,
            UserId = string.Empty
        });
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString() ?? "unknown";
        var clientType = Context.GetHttpContext()?.Request.Query["clientType"].ToString() ?? "unknown";
        Console.WriteLine(clientType + ": " + userId + " connected to GameHub.");
        connections[Context.ConnectionId] = (userId, clientType);

        await Groups.AddToGroupAsync(Context.ConnectionId, userId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        connections.TryRemove(Context.ConnectionId, out _);
        await base.OnDisconnectedAsync(exception);
    }

    // Optional helper to get all clients by type in a group
    public IEnumerable<string> GetClientsInGroup(string groupId, string clientType)
    {
        return connections
            .Where(c => c.Value.ClientType == clientType && YourGroupCheck(groupId, c.Key))
            .Select(c => c.Key);
    }

    private bool YourGroupCheck(string groupId, string connectionId)
    {
        // Optionally check if the connection is in a group (track it yourself if needed)
        return true; // Or implement tracking
    }
}