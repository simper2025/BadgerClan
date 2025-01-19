
namespace BadgerClan.Logic;

public class GameEngine
{

    public GameState ProcessTurn(GameState state, List<Move> moves)
    {

        foreach (var unit in state.Units.Where(u => u.Team == state.CurrentTeam))
        {
            unit.Moves = unit.MaxMoves;
        }

        foreach (var move in moves)
        {
            var unit = state.Units.FirstOrDefault(u => u.Id == move.unitId);
            if (unit == null || unit.Team != state.CurrentTeam)
            {
                continue;
            }
            var distance = unit.Location.Distance(move.target);
            var defender = state.Units.FirstOrDefault(u =>
                u.Location == move.target && u.Id != unit.Id);
            switch (move.Type)
            {
                case MoveType.Walk:
                    var movedLocation = new Coordinate(move.target.Q, move.target.R);
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
            }

            state.Units.RemoveAll(u => u.Health <= 0);
        }
        state.IncrementTurn();
        return state;
    }
}
