namespace BadgerClan.Logic.Bot;


public interface IBot
{
    List<Move> PlanMoves(GameState state);

}