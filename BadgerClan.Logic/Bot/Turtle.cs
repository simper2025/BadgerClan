
namespace BadgerClan.Logic.Bot;

public class Turtle : IBot
{
    public int Team;
    public Turtle(int team)
    {
        Team = team;
    }

    public List<Move> PlanMoves(GameState state)
    {
        var active = false;
        var enemies = state.Units.Where(u => u.Team != Team);

        foreach (var unit in state.Units.Where(u => u.Team == Team))
        {
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null && closest.Location.Distance(unit.Location) <= 5)
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
}