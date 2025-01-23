using BadgerClan.Logic;

namespace BadgerClan.Test.GameEngineTest;

public class AttackMoveTest
{
    private GameEngine engine;

    public AttackMoveTest()
    {
        engine = new GameEngine();
    }

    // [Fact]
    public void OnlyOneMoveAfterAttack()
    {
        var state = new GameState();
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(3, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var expected = knight1.Location.MoveSouthWest(1);
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location),
            new Move(MoveType.Walk, knight1.Id, expected),
            new Move(MoveType.Walk, knight1.Id, expected.MoveSouthWest(2)),
        };
        GameEngine.ProcessTurn(state, moves);

        Assert.Equal(expected, knight1.Location);
    }

    // [Fact]
    public void ArcherCanAttackOnceAndMoveTwoTimes()
    {
        var state = new GameState();
        var archer1 = Unit.Factory("Archer", 1, Coordinate.Offset(4, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(2, 2));
        state.AddUnit(archer1);
        state.AddUnit(knight2);
        var expected = archer1.Location.MoveSouthEast(2);
        var moves = new List<Move> {
            new Move(MoveType.Attack, archer1.Id, knight2.Location),
            new Move(MoveType.Walk, archer1.Id, archer1.Location.MoveSouthEast(1)),
            new Move(MoveType.Walk, archer1.Id, expected),
        };
        GameEngine.ProcessTurn(state, moves);

        Assert.Equal(expected, archer1.Location);
    }

    // [Fact]
    public void ArcherCanAttackOnceAndMoveTwo()
    {
        var state = new GameState();
        var archer1 = Unit.Factory("Archer", 1, Coordinate.Offset(4, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(2, 2));
        state.AddUnit(archer1);
        state.AddUnit(knight2);
        var expected = archer1.Location.MoveSouthEast(2);
        var moves = new List<Move> {
            new Move(MoveType.Attack, archer1.Id, knight2.Location),
            new Move(MoveType.Walk, archer1.Id, expected),
        };
        GameEngine.ProcessTurn(state, moves);

        Assert.Equal(expected, archer1.Location);
    }

    //  


}