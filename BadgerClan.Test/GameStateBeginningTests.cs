

using BadgerClab.Logic;

namespace BadgerClan.Test;

public class GameStateBeginningTests
{
    private GameEngine engine;

    public GameStateBeginningTests()
    {
        engine = new GameEngine();
        
    }

    [Fact]
    public void Test1()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", new Coordinate(2,2));
        state.AddUnit(knight);
        var moves = new List<Move> { new Move(MoveType.Walk, knight.Id, new Coordinate(3, 2))};
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(3, knight.Location.Col);
    }
}
