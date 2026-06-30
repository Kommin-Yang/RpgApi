using Microsoft.AspNetCore.SignalR;
using RpgApi.Data;
using RpgApi.Services;

namespace RpgApi.Hubs;

public class GameHub : Hub
{
    private readonly GameState _gameState;
    private readonly IServiceScopeFactory _scopeFactory;

    public GameHub(GameState gameState, IServiceScopeFactory scopeFactory)
    {
        _gameState = gameState;
        _scopeFactory = scopeFactory;
    }

    public async Task JoinGame(int characterId)
    {
        Context.Items["CharacterId"] = characterId;
        await Clients.Caller.SendAsync("ConnectedToWorld", $"Welcome, character {_gameState.GetPlayer(characterId)!.Character.Name}!");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        // Get Id of the disconnected player 
        if(Context.Items.TryGetValue("CharacterId", out var objId) && 
            objId is int characterId)
        {
            var player = _gameState.GetPlayer(characterId);

            if(player != null)
            {
                // Temporaly scope to use DBContext
                using (var scope = _scopeFactory.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<RpgDbContext>();

                    // Save
                    context.Characters.Update(player.Character);
                    await context.SaveChangesAsync();
                }
                // Remove player
                _gameState.RemovePlayer(characterId);
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}