using RpgApi.Data;

namespace RpgApi.Services;

public class SaveGameWorker : BackgroundService
{
    private readonly GameState _gameState;
    private readonly IServiceScopeFactory _scopeFactory;

    public SaveGameWorker(GameState gameState, IServiceScopeFactory scopeFactory)
    {
        _gameState = gameState;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            // Wait 1 minute between each server's save
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            
            // Create a scope to use DBContext (cause DbContext cant be a Singleton)
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RpgDbContext>();

            var activePlayers = _gameState.GetAllActivePlayers();

            foreach (var player in activePlayers)
            {
                // Notifying Entity Framework players in RAM change and have to be updated
                context.Characters.Update(player.Character);
            }

            // Big save for everyone each minutes
            await context.SaveChangesAsync(stoppingToken);
        }
    }
}