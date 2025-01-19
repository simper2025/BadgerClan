namespace BadgerClan.Logic;

public class GameEngine
{

    public GameState ProcessTurn(GameState state, List<Move> moves)
    {

        foreach (var unit in state.Units)
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
                    if (distance <= unit.Moves && defender == null &&
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
                    if (defender != null)
                    {
                        defender.Health = defender.Health - unit.Attack;
                        unit.Moves = unit.Moves - (unit.MaxMoves / 2);
                    }
                    break;
            }


        }
        state.IncrementTurn();
        return state;
    }
}
