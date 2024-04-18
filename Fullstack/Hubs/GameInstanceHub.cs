using Microsoft.AspNetCore.SignalR;
namespace FullstackApp.Hubs;

public class GameInstanceHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        Console.WriteLine("A client connected to the GameInstanceHub.");
        await base.OnConnectedAsync();
    }

    public async Task SendGameInstance(string gameInstance)
    {
        await Clients.All.SendAsync("ReceiveGameInstance", gameInstance);
    }
}

