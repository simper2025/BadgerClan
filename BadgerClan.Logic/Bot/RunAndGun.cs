
namespace BadgerClan.Logic.Bot;

public class RunAndGun : IBot
{
    public Task<List<Move>> PlanMovesAsync(GameState state)
    {
        var myteam = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
        if (myteam is null)
            return Task.FromResult(new List<Move>());

        var moves = new List<Move>();
        foreach (var unit in state.Units.Where(u => u.Team == state.CurrentTeamId))
        {
            var enemies = state.Units.Where(u => u.Team != state.CurrentTeamId);
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null)
            {

                if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                {
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                    moves.Add(SharedMoves.AttackClosest(unit, closest));
                }
                else if (myteam.Medpacs > 0 && unit.Health < unit.MaxHealth)
                {
                    moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
                }
                else
                {
                    moves.Add(SharedMoves.StepToClosest(unit, closest, state));
                }
            }
        }
        return Task.FromResult(moves);
    }

}