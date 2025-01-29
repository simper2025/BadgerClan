using BadgerClan.Logic;
using BadgerClan.Logic.Bot;
using System.Reflection.Emit;

namespace BadgerClan.Api.Moves;

public static class Strategies
{
    public static string Strategy = "";
    public static void RunAndGun(MoveRequest request, List<Move> moves)
    {
        foreach (var unit in request.Units.Where(u => u.Team == request.YourTeamId))
        {

            var enemies = request.Units.Where(u => u.Team != request.YourTeamId);
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null)
            {

                if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                {
                    moves.Add(Moves.AttackClosest(unit, closest));
                    moves.Add(Moves.AttackClosest(unit, closest));
                }
                else
                {
                    moves.Add(Moves.StepToClosest(unit, closest, request.Units));
                }
            }

        }
    }
    public static void Turtle(MoveRequest request, List<Move> moves)
    {

        var enemies = request.Units.Where(u => u.Team != request.YourTeamId);
        var squad = request.Units.Where(u => u.Team == request.YourTeamId);

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
                    moves.Add(Moves.AttackClosest(unit, closest));
                }
                else if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                {
                    moves.Add(Moves.AttackClosest(unit, closest));
                    moves.Add(Moves.AttackClosest(unit, closest));
                }
            }
        }
    }
}
