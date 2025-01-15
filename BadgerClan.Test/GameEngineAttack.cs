

using System.Security.Cryptography;
using BadgerClab.Logic;

namespace BadgerClan.Test;

public class GameEngineAttack
{
    private GameEngine engine;

    public GameEngineAttack()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void KnightAttacksKnight()
    {
        var state = new GameState();
        var knight1 = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", Coordinate.Offset(3, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var expectedHealth = knight2.Health - knight1.Attack;
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location)
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(expectedHealth, knight2.Health);
    }

    [Fact]
    public void UnitCantAttackSelf()
    {
        var state = new GameState();
        var knight1 = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", Coordinate.Offset(3, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var expectedHealth = knight1.Health;
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight1.Location)
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(expectedHealth, knight1.Health);
    }

    [Fact]
    public void KnightCanOnlyAttackOneSpaceAway()
    {
        var state = new GameState();
        var knight1 = Unit.Factory("Knight", Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", Coordinate.Offset(4, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var expectedHealth = knight2.Health;
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location)
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(expectedHealth, knight2.Health);
    }

    [Fact]
    public void ArcherCanAttackUpToThreeSpacesAway()
    {
        var state = new GameState();
        var archer = Unit.Factory("Archer", Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", Coordinate.Offset(5, 2));
        state.AddUnit(archer);
        state.AddUnit(knight2);
        var expectedHealth = knight2.Health - archer.Attack;
        var moves = new List<Move> {
            new Move(MoveType.Attack, archer.Id, knight2.Location)
        };
        var state2 = engine.ProcessTurn(state, moves);
        Assert.Equal(expectedHealth, knight2.Health);
    }

}
