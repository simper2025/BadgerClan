using BadgerClan.Logic;

namespace BadgerClan.Test.GameEngineTest;

public class MedpacTest
{
    private GameEngine engine;

    public MedpacTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void OnlyOneMoveAfterAttack()
    {
        var state = new GameState();
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.AddTeam(1, "Team1", Coordinate.Offset(10, 10), team);
        state.AddTeam(2, "Team2", Coordinate.Offset(20, 20), team);

        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(3, 2));
        knight2.Health = 1;
        state.AddUnit(knight1);
        state.AddUnit(knight2);

        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location),
        };
        var state2 = engine.ProcessTurn(state, moves);
    }
}