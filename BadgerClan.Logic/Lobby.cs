using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BadgerClan.Logic;

public class Lobby(ILogger<Lobby> logger)
{
    public const int TickInterval = 50;
    private Dictionary<Guid, List<GameState>> games { get; } = new();
    private Dictionary<int, int> failCount = new();
    public event Action<GameState>? LobbyChanged;
    public void AddGame(string gameName, Guid gameOwnerId)
    {
        var game = new GameState(gameName);
        if (games.ContainsKey(gameOwnerId) && games[gameOwnerId] != null)
        {
            games[gameOwnerId].Add(game);
        }
        else
        {
            games.Add(gameOwnerId, [game]);
        }
        LobbyChanged?.Invoke(game);
        game.GameEnded += (g) => LobbyChanged?.Invoke(g);
    }
    public ReadOnlyCollection<GameState> Games => games.Values.SelectMany(g => g).ToList().AsReadOnly();

    private List<string> startingUnits = new List<string> { "Knight", "Knight", "Archer", "Archer", "Knight", "Knight" };

    private Dictionary<Guid, CancellationTokenSource> gameTokens = new();

    public bool UserCreatedGame(Guid gameOwnerId, GameState game) => games.ContainsKey(gameOwnerId) && games[gameOwnerId].Any(g => g.Id == game.Id);

    public void StartGame(Guid gameOwnerId, GameState game)
    {
        if (UserCreatedGame(gameOwnerId, game))
        {
            game.LayoutStartingPositions(startingUnits);
            var source = new CancellationTokenSource();
            gameTokens[game.Id] = source;

            Task.Run(async () => await ProcessTurnAsync(game, source.Token), source.Token);
        }
    }

    public void StopGame(Guid gameCreatorId, GameState game)
    {
        if (UserCreatedGame(gameCreatorId, game) && gameTokens.ContainsKey(game.Id))
        {
            gameTokens[game.Id].Cancel();
            gameTokens.Remove(game.Id);
        }
    }

    private async Task ProcessTurnAsync(GameState game, CancellationToken ct)
    {
        while (game.Running || game.TurnNumber == 0)
        {
            ct.ThrowIfCancellationRequested();
            
            var moves = new List<Move>();

            if (!failCount.ContainsKey(game.CurrentTeamId) || failCount[game.CurrentTeamId] < 5)
            {
                try
                {
                    logger.LogInformation("Asking {team} for moves", game.CurrentTeam.Name);
                    moves = await game.CurrentTeam.PlanMovesAsync(game);
                    logger.LogInformation("Got {movecount} moves", moves.Count);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error getting moves for {team}", game.CurrentTeam.Name);
                    failCount.TryAdd(game.CurrentTeamId, 0);
                    failCount[game.CurrentTeamId]++;
                }
            }
            else
            {
                logger.LogWarning("Too many failed moves for {team}. Skipping to next player", game.CurrentTeam.Name);
            }
            GameEngine.ProcessTurn(game, moves);

            Thread.Sleep(TickInterval);
        }
    }
}