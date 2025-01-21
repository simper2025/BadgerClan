
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

    public int Team { get; set; }

    public Turtle()
    {
        Team = 0;

        // TurnsOfAction = 5;
        // TurnsOfDelay = 10;

        Random rnd = new Random();
        TurnsOfAction = rnd.Next(1, 5) + 5;
        TurnsOfDelay = rnd.Next(1, 10) + 5; 
    }

    public Turtle(int team) : this()
    {
        Team = team;
    }

    public Turtle(int action, int delay)
    {
        Team = 0;

        TurnsOfAction = action;
        TurnsOfDelay = delay;
    }

    public Turtle(int team, int action, int delay) : this(action, delay)
    {
        Team = team;
    }

    public List<Move> PlanMoves(GameState state)
    {
        var enemies = state.Units.Where(u => u.Team != Team);
        var active = ShouldGoActive(enemies);

        foreach (var unit in state.Units.Where(u => u.Team == Team))
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
        foreach (var unit in state.Units.Where(u => u.Team == Team && u.Type == "Knight"))
        {
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (active && closest != null)
            {
                if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                {
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                }
                else
                {
                    moves.Add(SharedMoves.StepToClosest(unit, closest, state));
                }
            }
        }
        foreach (var unit in state.Units.Where(u => u.Team == Team && u.Type == "Archer"))
        {
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (active && closest != null)
            {
                if (closest.Location.Distance(unit.Location) == 1)
                {
                    var target = unit.Location.Away(closest.Location);
                    moves.Add(new Move(MoveType.Walk, unit.Id, target));
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                }
                else if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                {
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
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