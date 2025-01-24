
namespace BadgerClan.Logic.Bot;

public class Turtle : IBot
{
	public int ActiveRange = 6;
	public int ActiveEnemyCount = 10;
	public int TurnsOfDelay = 1;
	public int TurnsOfAction = 1;

	private int LastEnemyCount = 0;
	private int TurnsSinceDeath = 0;
	private int ActionsLeft = 0;


	public static IBot Make()
	{
		return new Turtle();
	}
	public Turtle()
	{
		// TurnsOfAction = 5;
		// TurnsOfDelay = 10;

		Random rnd = new Random();
		TurnsOfAction = rnd.Next(1, 5) + 0;
		TurnsOfDelay = rnd.Next(1, 10) + 5;
	}

	public Turtle(int action, int delay)
	{
		TurnsOfAction = action;
		TurnsOfDelay = delay;
	}

	public async Task<List<Move>> PlanMovesAsync(GameState state)
	{
		var myteam = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
		if (myteam is null)
            return new List<Move>();

		var enemies = state.Units.Where(u => u.Team != state.CurrentTeamId);
		var active = ShouldGoActive(enemies);

		var squad = state.Units.Where(u => u.Team == state.CurrentTeamId);

		foreach (var unit in squad)
		{
			if (active) break;
			var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
			if (closest != null && closest.Location.Distance(unit.Location) <= ActiveRange)
			{
				active = true;
				break;
			}
		}

		var moves = new List<Move>();

		var pointman = squad.OrderBy(u => u.Id).FirstOrDefault();

		//Move knights first
		foreach (var unit in squad.OrderByDescending(u => u.Type == "Knight"))
		{
			var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
			if (closest != null)
			{
				if (pointman != null && unit.Id != pointman.Id &&
				unit.Location.Distance(pointman.Location) > 5)
				{
					//Don't split up
					var toward = unit.Location.Toward(pointman.Location);
					moves.Add(new Move(MoveType.Walk, unit.Id, toward));
					moves.Add(new Move(MoveType.Walk, unit.Id, toward.Toward(pointman.Location)));

				}
				else if (unit.Type == "Archer" && closest.Location.Distance(unit.Location) == 1)
				{
					//Archers run away from knights
					var target = unit.Location.Away(closest.Location);
					moves.Add(new Move(MoveType.Walk, unit.Id, target));
					moves.Add(SharedMoves.AttackClosest(unit, closest));
				}
				else if (active && closest.Location.Distance(unit.Location) <= unit.AttackDistance)
				{
					moves.Add(SharedMoves.AttackClosest(unit, closest));
					moves.Add(SharedMoves.AttackClosest(unit, closest));
				}
				else if (myteam.Medpacs > 0 && unit.Health < unit.MaxHealth)
				{
					moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
				}
				else if (active)
				{
					moves.Add(SharedMoves.StepToClosest(unit, closest, state));
				}
			}
		}

		return moves;
	}

	/*
        Go active
            if there are few enemies
            if it has been a few turns since someone died
        Stay active
            if you have active turns left
    */
	private bool ShouldGoActive(IEnumerable<Unit> enemies)
	{
		if (enemies.Count() < ActiveEnemyCount)
		{
			ActionsLeft = TurnsOfAction;
		}

		if (enemies.Count() == LastEnemyCount)
		{
			TurnsSinceDeath++;
		}
		else
		{
			LastEnemyCount = enemies.Count();
			TurnsSinceDeath = 0;
		}

		if (TurnsSinceDeath > TurnsOfDelay)
		{
			ActionsLeft = TurnsOfAction;
		}

		return ActionsLeft > 0;
	}
}