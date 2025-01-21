namespace BadgerClan.Logic.Bot;


public interface IBot
{
    int Team { get; set; }
    List<Move> PlanMoves(GameState state);

}