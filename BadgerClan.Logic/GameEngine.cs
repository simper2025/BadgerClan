namespace BadgerClab.Logic;

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
            if (unit == null)
            {
                continue;
            }
                    var distance = unit.Location.Distance(move.target);
            switch (move.Type)
            {
                case MoveType.Walk:
                    if (distance <= unit.Moves)
                    {
                        unit.Location = new Coordinate(move.target.Q, move.target.R);
                        unit.Moves -= distance;
                    }
                    break;
                case MoveType.Attack:
                    if (distance > unit.AttackDistance)
                    {
                        continue;
                    }
                    var defender = state.Units.FirstOrDefault(u => u.Location == move.target &&
                        u.Id != unit.Id);
                    if (defender != null)
                    {
                        defender.Health = defender.Health - unit.Attack;
                    }
                    break;
            }


        }

        return state;
    }
}
