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
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        state.AddUnits(1, Coordinate.Offset(10, 10), team);
        state.AddUnits(2, Coordinate.Offset(20, 20), team);

        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(3, 2));
        knight2.Health = 1;
        state.AddUnit(knight1);
        state.AddUnit(knight2);

        var moves = new List<Move> {
            new Move(MoveType.Attack, knight1.Id, knight2.Location),
        };
        state = engine.ProcessTurn(state, moves);
        Assert.Equal(13, state.Units.Count);
        Assert.True(state.TeamList[0].Medpacs > 0);
    }

    


}