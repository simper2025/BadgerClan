using BadgerClan.Logic;

namespace BadgerClan.Test.GameEngineTest;

public class MoveTest
{
    private BadgerClan.Logic.GameEngine engine;

    public MoveTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void MoveOneStep()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(1))
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(3, knight.Location.Col);
    }

    [Fact]
    public void ResetsMoves()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        knight.Moves = 0;
        state.AddUnit(knight);
        var moves = new List<Move> { };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.True(knight.Moves > 0);
    }

    [Fact]
    public void KnightCantMoveThree()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(1)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(2)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(3))
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(4, knight.Location.Col);
    }

    [Fact]
    public void CantMoveOffGridBottom(){
        var state = new GameState();
        var knight = Unit.Factory("Knight", Coordinate.Offset(1, 1));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveNorthEast(1)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveNorthEast(2)),
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(0, knight.Location.Row);
    }

[Fact]
    public void CantMoveOffGridSide(){
        var state = new GameState();
        state.Dimension = 6;
        var knight = Unit.Factory("Knight", Coordinate.Offset(5, 5));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveSouthWest(1)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveSouthWest(2)),
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(6, knight.Location.Row);
    }

}
