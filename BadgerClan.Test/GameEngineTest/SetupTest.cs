using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan.Test.GameEngineTest;

public class SetupTest
{
    private GameEngine engine;

    public SetupTest()
    {
        engine = new GameEngine();
    }

    [Fact]
    public void CreateGameWithName()
    {
        string expected = "Party Time";
        var state = new GameState(expected);
        Assert.Equal(expected, state.Name);
    }

    [Fact]
    public void CreateGameNoName()
    {
        string expected = "Game-";
        var state = new GameState();
        Assert.StartsWith(expected, state.Name);
        Assert.Equal(expected.Length + 4, state.Name.Length);
    }

    [Fact]
    public void CantAddSameUnitTwice()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        state.AddUnit(knight);
        state.AddUnit(knight);
        Assert.Single(state.Units);
    }

    [Fact]
    public void StackedUnitMoved()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        state.AddUnit(knight);

        var knight2 = Unit.Factory("Knight", 1, Coordinate.Offset(2, 2));
        state.AddUnit(knight2);
        
        Assert.Equal(2, state.Units.Count);
        Assert.NotEqual(Coordinate.Offset(2, 2), knight2.Location);
    }

    [Fact]
    public void StackedUnitNotMovedOffBoard()
    {
        var state = new GameState();
        state.Dimension = 6;
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        state.AddUnit(knight);
        state.AddUnit(knight2);
        Assert.True(knight2.Location.Col <= 6);
    }

    [Fact]
    public void OverloadedUnitsNotMovedOffBoard()
    {
        var state = new GameState();
        state.Dimension = 6;
        
        state.AddTeam(new Team(1));
        var loc = Coordinate.Offset(6, 6);
        var knight = Unit.Factory("Knight", 1, loc);
        state.AddUnit(knight);

        var unit = new List<string> { "Knight" };

        //for (int i = 0; i< state.Dimension*state.Dimension - 2; i++) {
        for (int i = 0; i < 6; i++) {
            state.AddUnits(1, loc, unit);
        }
        Assert.True(state.IsOnBoard(state.Units[state.Units.Count - 1].Location));

        //var knight2 = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        //state.AddUnit(knight2);
        //Assert.True(knight2.Location.Col <= 6);
    }

    [Fact]
    public void AddNextToTeammates()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(60, 60));
        state.AddUnit(knight);
        state.AddUnit(knight2);

        var knight3 = Unit.Factory("Knight", 1);
        state.AddUnit(knight3);

        Assert.Equal(3, state.Units.Count);
        Assert.Equal(1, knight3.Location.Distance(knight.Location));
    }

    [Fact]
    public void TurnCountAndChangeTeams()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(60, 60));
        state.AddUnit(knight);
        state.AddUnit(knight2);
        Assert.Equal(0, state.TurnNumber);
        Assert.Equal(1, state.CurrentTeamId);
        Assert.False(state.Running);

        GameEngine.ProcessTurn(state, new List<Move>());
        Assert.Equal(1, state.TurnNumber);
        Assert.Equal(2, state.CurrentTeamId);
        Assert.True(state.Running);

        GameEngine.ProcessTurn(state, new List<Move>());
        Assert.Equal(2, state.TurnNumber);
        Assert.Equal(1, state.CurrentTeamId);
    }


    [Fact]
    public void MoveOnlyOnYourTurn()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(60, 60));
        state.AddUnit(knight1);
        state.AddUnit(knight2);

        var expectedLocation1 = knight1.Location.MoveEast(1);
        var expectedLocation2 = knight2.Location.Copy();
        var moves = new List<Move>{
            new Move(MoveType.Walk, knight1.Id, knight1.Location.MoveEast(1)),
            new Move(MoveType.Walk, knight2.Id, knight2.Location.MoveEast(1)),
        };
        GameEngine.ProcessTurn(state, moves);

        Assert.Equal(expectedLocation1, knight1.Location);
        Assert.Equal(expectedLocation2, knight2.Location);

        expectedLocation1 = knight1.Location.Copy();
        expectedLocation2 = knight2.Location.MoveEast(1);
        moves = new List<Move>{
            new Move(MoveType.Walk, knight1.Id, knight1.Location.MoveEast(1)),
            new Move(MoveType.Walk, knight2.Id, knight2.Location.MoveEast(1)),
        };
        GameEngine.ProcessTurn(state, moves);

        Assert.Equal(expectedLocation1, knight1.Location);
        Assert.Equal(expectedLocation2, knight2.Location);
    }

    [Fact]
    public void TestGameOver()
    {
        var state = new GameState();
        var winner = new Team("Winner", "red", new NothingBot());
        state.AddTeam(winner);
        state.AddTeam(new Team(2));
        var knight1 = Unit.Factory("Knight", winner.Id, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(6, 5));
        state.AddUnit(knight1);
        knight2.Health = 1;
        state.AddUnit(knight2);

        var moves = new List<Move>{
            new Move(MoveType.Attack, knight1.Id, knight2.Location)
        };
        GameEngine.ProcessTurn(state, moves);

        Assert.Single(state.Units);
        Assert.False(state.Running);
        Assert.Contains("Winner", state.ToString());
    }

    [Fact]
    public void AddWholeSquad()
    {
        var state = new GameState();
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.AddTeam(new Team(1));
        state.AddUnits(1, Coordinate.Offset(10, 10), team);
        Assert.Contains(state.Units, u => u.Location == Coordinate.Offset(10, 10));
        Assert.Equal(6, state.Units.Count);
    }

    [Fact]
    public void StartGameWithUnits()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var team = new List<string> { "Knight", "Knight", "Knight", "Knight", "Archer", "Archer" };
        state.LayoutStartingPositions(team);
        Assert.Equal(team.Count * 2, state.Units.Count);
    }

    [Theory]
    [InlineData(0, 36, 10)]
    [InlineData(180, 36, 60)] //player 2 of 2
    [InlineData(120, 56, 47)] // 2 of 3
    [InlineData(2400, 14, 47)] // 3 of 3
    public void LayoutSquadsInCircle(int deg, int col, int row)
    {
        var loc = GameSetupHelper.GetCircleCoordinate(deg, 70);
        var expected = Coordinate.Offset(col, row);
        Assert.Equal(expected, loc);
    }



        [Fact]
    public void ResetGameAfterOver()
    {
        var state = new GameState();
        state.AddTeam(new Team(1));
        state.AddTeam(new Team(2));
        var knight1 = Unit.Factory("Knight", 1, Coordinate.Offset(6, 6));
        var knight2 = Unit.Factory("Knight", 2, Coordinate.Offset(6, 5));
        state.AddUnit(knight1);
        knight2.Health = 1;
        state.AddUnit(knight2);

        var moves = new List<Move>{
            new Move(MoveType.Attack, knight1.Id, knight2.Location)
        };
        GameEngine.ProcessTurn(state, moves);

        state.RestartGame();
        Assert.Empty(state.Units);
        Assert.Equal(0, state.TotalUnits);

        Assert.Equal(0, state.TurnNumber);
        
        Assert.False(state.Running);
        Assert.Equal(0, state.TeamList[0].Medpacs);

        Assert.Equal(1, state.CurrentTeamId);
        
    }

}
