
namespace BadgerClan.Logic.Bot;

public class Turtle : IBot
{
    public int Level;

    private int activeRange = 4;

    public int ActiveEnemyCount { get; private set; }
    public int LastEnemyCount { get; private set; }
    public int TurnsSinceDeath { get; private set; }

    public static IBot Make()
    {
        return new Turtle();
    }
    public Turtle()
    {
        Random rnd = new Random();
        Level = rnd.Next(1, 3);
    }

    public Turtle(int level)
    {
        Level = level;
    }

    public Task<List<Move>> PlanMovesAsync(GameState state)
    {
        var myteam = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
        if (myteam is null)
            return Task.FromResult(new List<Move>());

        var enemies = state.Units.Where(u => u.Team != state.CurrentTeamId);
        var active = ShouldGoActive(enemies, enemies.Select(u => u.Team).Distinct().Count());

        var squad = state.Units.Where(u => u.Team == state.CurrentTeamId);

        foreach (var unit in squad)
        {
            if (active) break;
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null && closest.Location.Distance(unit.Location) <= activeRange + Level)
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
                else if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
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

        return Task.FromResult(moves);
    }

    /*
        Go active
            if there are few enemies
            if it has been a few turns since someone died
        Stay active
            if you have active turns left
    */
    private bool ShouldGoActive(IEnumerable<Unit> enemies, int teams)
    {
        var active = false;
        if (enemies.Count() == LastEnemyCount)
        {
            TurnsSinceDeath++;
        }
        else
        {
            LastEnemyCount = enemies.Count();
            TurnsSinceDeath = 0;
        }

        if (enemies.Count() < 12)
        {
            active = true;
        }
        if (TurnsSinceDeath > (2 * teams) * (3 - Level))
        {
            active = true;

        }
        switch (Level)
        {
            case 1:


                break;
            case 2:


                break;

            case 3:

                break;
        }
        return active;
    }
}