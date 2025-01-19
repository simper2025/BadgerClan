
namespace BadgerClan.Logic;

public class MoveGenetator
{

    public static List<Move> MakeList(int team, GameState state)
    {
        Random rnd = new Random();

        var moves = new List<Move>();
        foreach (var unit in state.Units.Where(u => u.Team == team))
        {
            var enemies = state.Units.Where(u => u.Team != team);
            var closest = enemies.OrderBy(u => u.Location.Distance(unit.Location)).FirstOrDefault();
            if (closest != null)
            {

                if (closest.Location.Distance(unit.Location) <= unit.AttackDistance)
                {
                    var attack = new Move(MoveType.Attack, unit.Id, closest.Location);
                    moves.Add(attack);
                }
                else
                {
                    var r = 0;
                    if (closest.Location.R - unit.Location.R < 0)
                        r = -1;
                    else if (closest.Location.R - unit.Location.R > 0)
                        r = +1;

                    var q = 0;
                    if (closest.Location.Q - unit.Location.Q < 0)
                        q = -1;
                    else if (closest.Location.Q - unit.Location.Q > 0)
                        q = +1;

                    var target = new Coordinate(unit.Location.Q + q, unit.Location.R +r);

                    while (state.Units.Any(u => u.Location == target))
                    {
                        r = rnd.Next(-1, 1);
                        q = rnd.Next(-1, 1);
                        target = new Coordinate(unit.Location.Q + q, unit.Location.R +r);
                    }

                    var move = new Move(MoveType.Walk, unit.Id, target);
                    moves.Add(move);
                }
            }
        }
        return moves;
    }

}