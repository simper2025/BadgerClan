
namespace BadgerClan.Logic.Bot;

public class RunAndGun : IBot
{
    public int Team;
    public RunAndGun(int team)
    {
        Team = team;
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
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                }
                else
                {
                    moves.Add(SharedMoves.StepToClosest(unit, closest, state));
                }
            }
        }
        return moves;
    }

}