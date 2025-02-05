
namespace BadgerClan.Logic.Bot;

public class DevBot : IBot
{
    public int Team { get; set; }

    public Task<List<Move>> PlanMovesAsync(GameState state) {
        var moves = new List<Move>();
        


        
        
        
        return Task.FromResult(moves);
    }
}