using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;

namespace BadgerClan.Logic;

public class Lobby(ILogger<Lobby> logger)
{
    public const int TickInterval = 50;
    private List<GameState> games { get; } = new();
    public event Action<GameState>? LobbyChanged;
    public void AddGame(string gameName)
    {
        var game = new GameState(gameName);
        games.Add(game);
        LobbyChanged?.Invoke(game);
        game.GameEnded += (g) => LobbyChanged?.Invoke(g);
    }
    public ReadOnlyCollection<GameState> Games => games.AsReadOnly();

    private List<string> startingUnits = new List<string> { "Knight", "Knight", "Archer", "Archer", "Knight", "Knight" };

    private Dictionary<Guid, CancellationTokenSource> gameTokens = new();

    public void StartGame(GameState game)
    {
        game.LayoutStartingPositions(startingUnits);
        var source = new CancellationTokenSource();
        gameTokens[game.Id] = source;

        Task.Run(async () => await ProcessTurnAsync(game, source.Token), source.Token);
    }
    private async Task ProcessTurnAsync(GameState game, CancellationToken ct)
    {
        while (game.Running || game.TurnNumber == 0)
        {
            ct.ThrowIfCancellationRequested();

            logger.LogInformation("Asking {team} for moves", game.CurrentTeam.Name);
            try
            {
                var moves = await game.CurrentTeam.PlanMovesAsync(game);
                GameEngine.ProcessTurn(game, moves);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting moves for {team}", game.CurrentTeam.Name);
                return;
            }

            Thread.Sleep(TickInterval);
        }
    }
}