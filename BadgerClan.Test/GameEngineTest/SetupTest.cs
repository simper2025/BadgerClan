using BadgerClan.Logic;

namespace BadgerClan.Test.GameEngineTest;

public class SetupTest
{
    private GameEngine engine;

    public SetupTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void CantAddSameUnitTwice()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        state.AddUnit(knight);
        state.AddUnit(knight);
        Assert.Single(state.Units);
    }

    [Fact]
    public void StackedUnitMoved()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        state.AddUnit(knight);
        state.AddUnit(knight2);
        Assert.Equal(2, state.Units.Count);
        Assert.NotEqual(Coordinate.Offset(2,2), knight2.Location);
    }
    
    [Fact]
    public void StackedUnitNotMovedOffBoard()
    {
        var state = new GameState();
        state.Dimension = 6;
        var knight = Unit.Factory("Knight", Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", Coordinate.Offset(6, 6));
        state.AddUnit(knight);
        state.AddUnit(knight2);
        Assert.True(knight2.Location.Col <= 6);
    }

    //teams

    //add a unit with no location, drop next to team

    // game win lose


}
