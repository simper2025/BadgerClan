namespace BadgerClan.Logic.Bot;


public class SharedMoves
{
    public static Move AttackClosest(Unit unit, Unit closest)
    {
        var attack = new Move(MoveType.Attack, unit.Id, closest.Location);
        return attack;
    }

    public static Move StepToClosest(Unit unit, Unit closest, GameState state)
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
            }
        }

        var move = new Move(MoveType.Walk, unit.Id, target);
        return move;
    }


    public static bool CanAttack(Unit mine, Unit closest)
    {
        return closest.Location.Distance(mine.Location) <= mine.Attack;
    }

}