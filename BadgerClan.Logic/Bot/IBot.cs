namespace BadgerClan.Logic.Bot;


public interface IBot
{
    List<Move> PlanMoves(int team, GameState state);

}