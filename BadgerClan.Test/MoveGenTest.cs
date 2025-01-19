
using BadgerClan.Logic;

namespace BadgerClan.Test;


public class MoveGenTest
{

    private GameEngine engine;

    public MoveGenTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void Test1()
    {
        var state = new GameState();
        var archer1 = Unit.Factory("Archer", 1, Coordinate.Offset(10, 10));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(20, 20));
        state.AddUnit(archer1);
        state.AddUnit(knight2);

        var moves = MoveGenetator.MakeList(1, state);
        state = engine.ProcessTurn(state, moves);
        Assert.True(true);
    }

    [Fact]
    public void Test2()
    {
        var state = new GameState();
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.AddTeam(1, Coordinate.Offset(10, 10), team);
        state.AddTeam(2, Coordinate.Offset(30, 30), team);

        var moves = MoveGenetator.MakeList(1, state);
        Assert.Equal(6, moves.Count);
        state = engine.ProcessTurn(state, moves);

        moves = MoveGenetator.MakeList(2, state);
        Assert.Equal(6, moves.Count);
        state = engine.ProcessTurn(state, moves);
    }
}