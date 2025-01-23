namespace BadgerClan.Logic.Bot;


public interface IBot
{
    Task<List<Move>> PlanMovesAsync(GameState state);

}