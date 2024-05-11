using Microsoft.AspNetCore.SignalR;
namespace FullstackApp.Hubs;

public class GameHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("A client connected to the Game Hub.");
        await base.OnConnectedAsync();
    }

    public async Task SendGameInstance(string gameInstance)
    {
        await Clients.All.SendAsync("ReceiveGameInstance", gameInstance);
    }
}

