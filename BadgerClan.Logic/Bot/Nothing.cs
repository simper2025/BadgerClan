
namespace BadgerClan.Logic.Bot;

public class Nothing : IBot
{
    public int Team { get; set; }

    public List<Move> PlanMoves(GameState state)
    {
        return new List<Move>();
    }
}