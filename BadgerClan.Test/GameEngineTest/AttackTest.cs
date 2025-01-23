using BadgerClan.Logic;

namespace BadgerClan.Test.GameEngineTest;

public class AttackTest
{
    private GameEngine engine;

    public AttackTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void KnightAttacksKnight()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(3, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var expectedHealth = knight2.Health - knight1.Attack;
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location)
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(expectedHealth, knight2.Health);
    }

    [Fact]
    public void UnitCantAttackSelf()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(3, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var expectedHealth = knight1.Health;
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight1.Location)
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(2, state.Units.Count);
        Assert.Equal(expectedHealth, knight1.Health);
    }

    [Fact]
    public void KnightCanOnlyAttackOneSpaceAway()
    {
        var state = new GameState();
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(4, 2));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        var expectedHealth = knight2.Health;
        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location)
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(expectedHealth, knight2.Health);
    }

    [Fact]
    public void ArcherCanAttackUpToThreeSpacesAway()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var archer = Unit.Factory("Archer", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(5, 2));
        state.AddUnit(archer);
        state.AddUnit(knight2);
        var expectedHealth = knight2.Health - archer.Attack;
        var moves = new List<Move> {
            new Move(MoveType.Attack, archer.Id, knight2.Location)
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(expectedHealth, knight2.Health);
    }

}
