using BadgerClan.Logic;

namespace BadgerClan.Test.GameEngineTest;

public class AttackMoveTest
{
    private GameEngine engine;

    public AttackMoveTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void OnlyOneMoveAfterAttack()
    {
        var state = new GameState();
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(3, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location),
            new Move(MoveType.Walk, knight1.Id, knight1.Location.MoveSouthWest(1)),
            new Move(MoveType.Walk, knight1.Id, knight1.Location.MoveSouthWest(2)),
        };
        var state2 = engine.ProcessTurn(state, moves);

        Assert.Equal(Coordinate.Offset(1, 3), knight1.Location);
    }

    //attacks take movement

    
}