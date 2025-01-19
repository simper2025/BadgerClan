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
        Assert.NotEqual(Coordinate.Offset(2, 2), knight2.Location);
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

    [Fact]
    public void TeamsExist()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(6, 6));
        state.AddUnit(knight);
        state.AddUnit(knight2);
        Assert.Equal(2, state.Units.Count);
    }

    [Fact]
    public void AddNextToTeammates()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(60, 60));
        state.AddUnit(knight);
        state.AddUnit(knight2);
        var knight3 = Unit.Factory("Knight", 1);
        state.AddUnit(knight3);

        Assert.Equal(3, state.Units.Count);
        Assert.Equal(1, knight3.Location.Distance(knight.Location));
    }

    // game win lose



    //Turn Changing

}
