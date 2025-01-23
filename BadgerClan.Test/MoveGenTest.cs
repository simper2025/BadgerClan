
using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan.Test;


public class MoveGenTest
{

    private GameEngine engine;

    public MoveGenTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void BasicTest()
    {
        var state = new GameState();
        var bot = new RunAndGun(1);
        var archer1 = Unit.Factory("Archer", 1, Coordinate.Offset(10, 10));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(20, 20));
        state.AddUnit(archer1);
        state.AddUnit(knight2);

        var moves = bot.PlanMoves(state);
        state = engine.ProcessTurn(state, moves);
        Assert.True(true);
    }

    [Fact]
    public void OneTurn()
    {
        var state = new GameState();
        var bot = new RunAndGun(1);
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.AddTeam(1, "Team1", Coordinate.Offset(10, 10), team);
        state.AddTeam(2, "Team2", Coordinate.Offset(30, 30), team);

        var moves = bot.PlanMoves(state);
        Assert.Equal(6, moves.Count);
        state = engine.ProcessTurn(state, moves);
    }


    //[Fact]
    public void TwoTurns()
    {
        var state = new GameState();
        var bot1 = new RunAndGun(1);
        var bot2 = new RunAndGun(2);
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.AddTeam(1, "Team1", Coordinate.Offset(10, 10), team);
        state.AddTeam(2, "Team2", Coordinate.Offset(30, 30), team);

        var moves = bot1.PlanMoves(state);
        Assert.Equal(6, moves.Count);
        state = engine.ProcessTurn(state, moves);

        moves = bot2.PlanMoves(state);
        Assert.Equal(6, moves.Count);
        state = engine.ProcessTurn(state, moves);
    }
}