
namespace BadgerClan.Logic.Bot;

public class NothingBot : IBot
{
    public int Team { get; set; }

    public Task<List<Move>> PlanMovesAsync(GameState state)
    {
        return Task.FromResult(new List<Move>());
    }
}