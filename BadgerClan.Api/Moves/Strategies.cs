using BadgerClan.Logic;

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
}
