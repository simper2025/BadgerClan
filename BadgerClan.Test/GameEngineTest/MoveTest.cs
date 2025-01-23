using BadgerClan.Logic;

namespace BadgerClan.Test.GameEngineTest;

public class MoveTest
{
    private GameEngine engine;

    public MoveTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void MoveOneStep()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(1))
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(Coordinate.Offset(3, 2), knight.Location);
    }

    [Fact]
    public void ResetsMoves()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        knight.Moves = 0;
        state.AddUnit(knight);
        var moves = new List<Move> { };
        GameEngine.ProcessTurn(state, moves);
        Assert.True(knight.Moves > 0);
    }

    // rewrite to use a for loop and the knights moves
    [Fact]
    public void KnightCantMoveThree()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(1)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(2)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveEast(3))
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(Coordinate.Offset(4, 2), knight.Location);
    }

    [Fact]
    public void CantMoveOffGridBottom()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(1, 1));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveNorthEast(1)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveNorthEast(2)),
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(Coordinate.Offset(2, 0), knight.Location);
    }

    [Fact]
    public void CantMoveOffGridSide()
    {
        var state = new GameState();
        state.Dimension = 6;
        state.AddTeam(new Team(1));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(5, 5));
        state.AddUnit(knight);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveSouthWest(1)),
            new Move(MoveType.Walk, knight.Id, knight.Location.MoveSouthWest(2)),
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(Coordinate.Offset(5, 6), knight.Location);
    }

    [Fact]
    public void CantMoveOntoAnotherUnit()
    {
        var state = new GameState();
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(5, 5));
        var knight2 = Unit.Factory("Knight", 1, Coordinate.Offset(5, 6));
        var expectedLocation = knight.Location.Copy();
        state.AddUnit(knight);
        state.AddUnit(knight2);
        var moves = new List<Move> {
            new Move(MoveType.Walk, knight.Id, knight2.Location),
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(expectedLocation, knight.Location);
    }

}
