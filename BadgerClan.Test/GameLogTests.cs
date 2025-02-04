
using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan.Test;


public class GameLogTests
{

  [Fact]
  public async Task MovingProducesLog()
  {
    var bot1 = new RunAndGun();
    var bot2 = new RunAndGun();
    var team1 = new Team("Team 1", "red", bot1);
    var team2 = new Team("Team 2", "red", bot2);
    var testState = new GameState()
    {
      Units = [
        Unit.Factory("Knight", team1.Id, Coordinate.Offset(0, 0)),
        Unit.Factory("Knight", team2.Id,  Coordinate.Offset(0, 0)),
      ],
      TeamList = [
        team1, 
        team2
      ]
    };

    var state = new GameState();
    state.AddTeam(team1);
    state.AddTeam(team2);

    var squad = new List<string> { "Knight" };
    state.LayoutStartingPositions(squad);


    var moves = await bot1.PlanMovesAsync(state);
    var unitStartingLocation = state.Units.First(u => u.Team == team1.Id).Location;

    GameEngine.ProcessTurn(state, moves);

    Assert.Single(state.Logs);
    var log = state.Logs[0];
    Assert.Equal(moves[0].UnitId, log.UnitId);

    Assert.NotNull(log.SourceCoordinate);
    Assert.Equal(unitStartingLocation.Q, log.SourceCoordinate!.Q);
    Assert.Equal(unitStartingLocation.R, log.SourceCoordinate!.R);


    Assert.NotNull(log.DestinationCoordinate);
    Assert.Equal(moves[0].Target!.Q, log.DestinationCoordinate!.Q);
    Assert.Equal(moves[0].Target!.R, log.DestinationCoordinate!.R);
  }
}