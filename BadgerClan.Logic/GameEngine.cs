

namespace BadgerClan.Logic;

public class GameEngine
{

    public static void ProcessTurn(GameState state, List<Move> moves)
    {
        var team = state.TeamList.FirstOrDefault(t => t.Id == state.CurrentTeamId);
        if (team is null)
        {
            state.IncrementTurn();
            return;
        }
        foreach (var unit in state.Units.Where(u => u.Team == state.CurrentTeamId))
        {
            unit.Moves = unit.MaxMoves;
        }

        foreach (var move in moves)
        {
            var unit = state.Units.FirstOrDefault(u => u.Id == move.UnitId);
            if (unit == null || unit.Team != state.CurrentTeamId)
            {
                continue;
            }
            var distance = unit.Location.Distance(move.Target);
            var defender = state.Units.FirstOrDefault(u =>
                u.Location == move.Target && u.Id != unit.Id);
            switch (move.Type)
            {
                case MoveType.Walk:
                    var movedLocation = new Coordinate(move.Target.Q, move.Target.R);
                    var canMove = distance <= unit.Moves;
                    if (!canMove)
                    {
                        if (distance <= unit.Moves + (1 / 2.0 + 0.01))
                        {
                            canMove = true;
                        }
                    }
                    if (canMove && defender == null &&
                        state.IsOnBoard(movedLocation))
                    {
                        unit.Location = movedLocation;
                        unit.Moves -= distance;
                    }
                    break;

                case MoveType.Attack:
                    if (distance > unit.AttackDistance)
                    {
                        continue;
                    }
                    var attackCost = unit.MaxMoves / unit.AttackCount;
                    if (defender != null && unit.Moves > (attackCost / 2.0))
                    {
                        defender.Health -= unit.Attack;
                        unit.Moves -= attackCost;
                    }
                    break;

                case MoveType.Medpac:
                    if (team.Medpacs > 0 && unit.Health < unit.MaxHealth)
                    {
                        unit.Moves--;
                        if (unit.MaxHealth - unit.Health > team.Medpacs)
                        {
                            unit.Health += team.Medpacs;
                            team.Medpacs = 0;
                        }
                        else
                        {
                            var health = unit.MaxHealth - unit.Health;
                            unit.Health = unit.MaxHealth;
                            team.Medpacs -= health;
                        }
                    }
                    break;
            }
            var deadunits = state.Units.Count(u => u.Health <= 0);
            for (int i = 0; i < deadunits; i++)
            {
                int meds = CalculateMeds(state.Units.Count, state.TotalUnits);
                team.Medpacs++;
            }
            state.Units.RemoveAll(u => u.Health <= 0);
        }
        state.IncrementTurn();
    }

    public static int CalculateMeds(int unitsLeft, int totalUnits)
    {
        return (unitsLeft + 5) / 10;
    }
}
