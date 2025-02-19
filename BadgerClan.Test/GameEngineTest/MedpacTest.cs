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
    public void GameStateTellMedpac()
    {
        var state = new GameState();
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        state.AddUnits(1, Coordinate.Offset(10, 10), team);
        state.AddUnits(2, Coordinate.Offset(20, 20), team);

        Assert.Equal(10, state.NextMedpac);
    }

    [Fact]
    public void GetMedpacAfterKill()
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
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(13, state.Units.Count);
        Assert.True(state.TeamList[0].Medpacs > 1);
        Assert.Equal(10, state.TeamList[0].Medpacs);
    }

    [Fact]
    public void ApplyMedpacToHeal()
    {

        var state = new GameState();
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        var team1 = new Team(1);
        team1.Medpacs = 1;
        state.AddTeam(team1);
        state.AddTeam(new Team(2));
        state.AddUnits(1, Coordinate.Offset(10, 10), team);
        state.AddUnits(2, Coordinate.Offset(20, 20), team);

        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        knight1.Health = 1;
        state.AddUnit(knight1);

        var moves = new List<Move> {
            new Move(MoveType.Medpac, knight1.Id, knight1.Location),
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(2, knight1.Health);
        Assert.Equal(0, team1.Medpacs);
    }

    [Fact]
    public void SomeMedpacsLeftOver()
    {

        var state = new GameState();
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        var team1 = new Team(1);
        team1.Medpacs = 5;
        state.AddTeam(team1);
        state.AddTeam(new Team(2));
        state.AddUnits(1, Coordinate.Offset(10, 10), team);
        state.AddUnits(2, Coordinate.Offset(20, 20), team);

        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        knight1.Health = knight1.MaxHealth - 3;
        state.AddUnit(knight1);

        var moves = new List<Move> {
            new Move(MoveType.Medpac, knight1.Id, knight1.Location),
        };
        GameEngine.ProcessTurn(state, moves);
        Assert.Equal(knight1.MaxHealth, knight1.Health);
        Assert.Equal(2, team1.Medpacs);
    }

    [Theory]
    [InlineData(49, 50, 9)]
    // [InlineData(47, 50, 14)]
    // [InlineData(28, 50, 5)]
    [InlineData(7, 50, 0)]
    [InlineData(2, 50, 0)]
    [InlineData(1, 50, 0)]
    public void TestMedpacDirectly(int remaining, int start, int expected)
    {
        var meds = GameEngine.CalculateMeds(remaining, start);
        Assert.Equal(expected, meds);
    }




}