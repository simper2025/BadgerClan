
using BadgerClan.Logic;
using BadgerClan.Logic.Bot;

namespace BadgerClan.Test;


public class GameLogTests
{

  [Fact]
  public async Task MovingProducesLog()
  {
    var state = new GameState();

    var bot1 = new RunAndGun();
    var team1 = new Team("Team 1", "red", bot1);
    state.AddTeam(team1);

    var bot2 = new RunAndGun();
    var team2 = new Team("Team 2", "red", bot2);
    state.AddTeam(team2);

    var squad = new List<string> { "Knight" };
    state.LayoutStartingPositions(squad);


    var moves = await bot1.PlanMovesAsync(state);

    GameEngine.ProcessTurn(state, moves);

    Assert.Single(state.Logs);

  }
}