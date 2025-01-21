
namespace BadgerClan.Logic.Bot;

public class RunAndGun : IBot
{
    public int Team;
public RunAndGun(int team)
{
    Team = team;
}
    private static Move AttackClosest(Unit unit, Unit closest)
    {
        var attack = new Move(MoveType.Attack, unit.Id, closest.Location);
        return attack;
    }

    private static Coordinate Away(Unit unit, Unit closest)
    {
        var r = 0;
        if (closest.Location.R - unit.Location.R < 0)
            r = +1;
        else if (closest.Location.R - unit.Location.R > 0)
            r = -1;

        var q = 0;
        if (closest.Location.Q - unit.Location.Q < 0)
            q = +1;
        else if (closest.Location.Q - unit.Location.Q > 0)
            q = -1;

        var target = new Coordinate(unit.Location.Q + q, unit.Location.R + r);
        return target;
    }

    private static Move StepToClosest(Unit unit, Unit closest, GameState state)
    {
        Random rnd = new Random();

        var target = unit.Location.Toward(closest.Location);

        var neighbors = unit.Location.Neighbors();

        while (state.Units.Any(u => u.Location == target))
        {
            if (neighbors.Any())
            {
                var i = rnd.Next(0, neighbors.Count() - 1);
                target = neighbors[i];
                neighbors.RemoveAt(i);
            }
            else
            {
                neighbors = unit.Location.MoveEast(1).Neighbors();
                // r = rnd.Next(-1, 1);
                // q = rnd.Next(-1, 1);
                // target = new Coordinate(unit.Location.Q + q, unit.Location.R + r);
            }
        }

        var move = new Move(MoveType.Walk, unit.Id, target);
        return move;
    }

    public List<Move> PlanMoves(GameState state)
    {

        var moves = new List<Move>();
        foreach (var unit in state.Units.Where(u => u.Team == Team))
        {
            var enemies = state.Units.Where(u => u.Team != Team);
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null)
            {

                if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                {
                    moves.Add(AttackClosest(unit, closest));
                    moves.Add(AttackClosest(unit, closest));
                }
                else
                {
                    moves.Add(StepToClosest(unit, closest, state));
                }
            }
        }
        return moves;
    }

}