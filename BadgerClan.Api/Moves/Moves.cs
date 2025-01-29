using BadgerClan.Logic;
using System.Reflection.Emit;

namespace BadgerClan.Api.Moves;

public static class Moves
{
    public static Move StepToClosest(UnitDto unit, UnitDto closest, IEnumerable<UnitDto> Units)
    {
        Random rnd = new Random();

        var target = unit.Location.Toward(closest.Location);

        var neighbors = unit.Location.Neighbors();

        while (Units.Any(u => u.Location == target))
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

    public static Move AttackClosest(UnitDto unit, UnitDto closest)
    {
        var attack = new Move(MoveType.Attack, unit.Id, closest.Location);
        return attack;
    }
}