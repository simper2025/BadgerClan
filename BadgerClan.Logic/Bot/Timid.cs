
namespace BadgerClan.Logic.Bot;

public class Timid : IBot
{
    public Task<List<Move>> PlanMovesAsync(GameState state)
    {
        var myteam = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
        if (myteam is null)
            return Task.FromResult(new List<Move>());

        var team = state.Units.Where(u => u.Team == state.CurrentTeamId);
        var leader = team.OrderBy(u => u.Id).ToList()[0];

        var distanceToLeader = state.Units.Where(u => u.Team != myteam.Id)
            .OrderBy(u => u.Location.Distance(leader.Location))
            .First().Location.Distance(leader.Location);

        var moves = new List<Move>();
        foreach (var unit in team)
        {
            var enemies = state.Units.Where(u => u.Team != state.CurrentTeamId);
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();

            bool usedHealh = false;

            if (closest != null)
            {
                for (int i = 0; i < unit.MaxMoves; i++)
                {
                    if (distanceToLeader <= 5 && 
                        closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                    {
                        moves.Add(SharedMoves.AttackClosest(unit, closest));
                    }
                    else if (!usedHealh && myteam.Medpacs > 0 && unit.Health < unit.MaxHealth/2)
                    {
                        usedHealh = true;
                        moves.Add(new Move(MoveType.Medpac, unit.Id, unit.Location));
                    }
                    else if (leader.Location.Distance(unit.Location) > 5)
                    {
                        moves.Add(new Move(MoveType.Walk, unit.Id, unit.Location.Toward(leader.Location)));
                    }
                    else if(distanceToLeader > 5)
                    {
                        //moves.Add(new Move(MoveType.Walk, ))
                    }
                    else
                    {
                        moves.Add(SharedMoves.StepToClosest(unit, closest, state));
                    }
                }
            }
        }
        return Task.FromResult(moves);
    }

}