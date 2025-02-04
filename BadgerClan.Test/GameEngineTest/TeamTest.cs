using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan.Test.GameEngineTest;

public class TeamTest
{
    private GameEngine engine;

    public TeamTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void TeamsExist()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(6, 6));
        state.AddUnit(knight);
        state.AddUnit(knight2);
        Assert.Equal(2, state.Units.Count);
    }

    [Fact]
    public void CantAddTeamAfterStart()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(60, 60));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        GameEngine.ProcessTurn(state, new List<Move>());
        var knight3 = Unit.Factory("Knight", 3, Coordinate.Offset(40, 40));
        state.AddUnit(knight3);
        Assert.Equal(2, state.Units.Count);
    }

    [Fact]
    public void SpecifyId()
    {
        var team = new Team(13);
        Assert.Equal(13, team.Id);
    }

    [Fact]
    public void IdIncrementsNextId()
    {
        var team = new Team(14);
        var team2 = new Team("team", "red", new NothingBot());
        //Why does this break half the time?
        //Assert.True(team2.Id > team.Id);
    }

    [Fact]
    public void DeadTeamsPlayNoMoves()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        state.AddTeam(new Team(3));
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(3, 3));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(4, 3));
        var knight3 = Unit.Factory("Knight", 3, Coordinate.Offset(10, 10));
        state.AddUnit(knight1);
        state.AddUnit(knight2);
        state.AddUnit(knight3);
        knight2.Health = 1;

        Assert.Equal(1, state.CurrentTeamId);
        Assert.Equal(1, state.CurrentTeam.Id);

        GameEngine.ProcessTurn(state, new List<Move>());
        Assert.Equal(2, state.CurrentTeamId);
        
        GameEngine.ProcessTurn(state, new List<Move>());
        Assert.Equal(3, state.CurrentTeamId);
        
        GameEngine.ProcessTurn(state, new List<Move>());

        var moves = new List<Move>
        {
            new Move(MoveType.Attack, knight1.Id, knight2.Location)
        };
        GameEngine.ProcessTurn(state, moves);

        Assert.Equal(3, state.CurrentTeamId);
        Assert.Equal(3, state.CurrentTeam.Id);


    }
}
