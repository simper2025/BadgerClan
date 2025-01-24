
namespace BadgerClan.Logic.Bot;

public class NothingBot : IBot
{
    public int Team { get; set; }

	public async Task<List<Move>> PlanMovesAsync(GameState state)
    {
        return new List<Move>();
    }
}