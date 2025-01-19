
namespace BadgerClan.Logic;


public class GameState
{
    public int Dimension = 100;

    public List<Unit> Units { get; set; }

    public GameState()
    {
        Units = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        if (!Units.Contains(unit))
        {
            if (!IsOnBoard(unit.Location))
            {
                if (Units.Any(u => u.Team == unit.Team))
                    unit.Location = Units.FirstOrDefault(u => u.Team == unit.Team)?.Location
                        ?? Coordinate.Offset(0, 0);
                else
                    unit.Location = Coordinate.Offset(0, 0);
            }
            while (Units.Any(u => u.Location == unit.Location))
            {
                var target = unit.Location.MoveEast(1);
                if (!IsOnBoard(target))
                {
                    target = unit.Location.MoveSouthWest(1);
                }
                unit.Location = target;
            }
            Units.Add(unit);

        }
    }

    public bool IsOnBoard(Coordinate loc)
    {
        return loc.Col >= 0 && loc.Row >= 0 &&
            loc.Col <= Dimension && loc.Row <= Dimension;

    }
}