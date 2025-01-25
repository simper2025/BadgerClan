
namespace BadgerClan.Logic.Bot;

public class NothingBot : IBot
{
    public int Team { get; set; }

    public Task<List<Move>> PlanMovesAsync(GameState state) => Task.FromResult(new List<Move>());
}