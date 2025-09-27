public class TokenCleanupService : BackgroundService
{
    private readonly TokenStorage _tokenStore;

    public TokenCleanupService(TokenStorage tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _tokenStore.CleanupExpiredTokens();
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}