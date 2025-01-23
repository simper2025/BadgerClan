
namespace BadgerClan.Logic;


public class GameState
{
    private static int NextId = 1;
    public string Name { get; set; }
    public int Dimension = 70;

    public List<Unit> Units { get; set; }
    private List<int> Teams;

    public int TeamCount { get { return Teams.Count(); } }

    public int Turn { get; private set; }

    private int currentTeam = 0;
    public int CurrentTeam
    {
        get
        {
            return currentTeam;
        }
    }

    public bool Running
    {
        get
        {
            if (Turn == 0)
                return false;
            return Units.Select(u => u.Team).Distinct().Count() > 1;
        }
    }

    public GameState(string? name = null)
    {
        Units = new List<Unit>();
        Teams = new List<int>();
        Turn = 0;
        Name = name ?? $"Game{NextId++}";
    }

    public override string ToString()
    {
        string status = "Turn #" + Turn + "; ";
        if (Running)
        {
            foreach (int team in Teams)
            {
                status += "Team " + team + ": " + Units.Count(u => u.Team == team) + "; ";
            }
        }
        else if (Turn > 0)
        {
            var team = Units.FirstOrDefault()?.Team ?? 0;
            status = "GameOver; Team #" + team + " wins";
        }
        return status;
    }

    public void IncrementTurn()
    {
        var teamIndex = Teams.IndexOf(currentTeam);
        if (teamIndex != Teams.Count - 1)
            teamIndex++;
        else
            teamIndex = 0;
        currentTeam = Teams[teamIndex];

        Turn++;
    }

    public void AddTeam(int team, Coordinate loc, List<string> units)
    {
        foreach (var unit in units)
        {
            AddUnit(Unit.Factory(unit, team, loc));
        }
    }

    public void AddUnit(Unit unit)
    {
        if (Units.Contains(unit))
        {
            return;
        }
        if (Turn > 0 && !Units.Any(u => u.Team == unit.Team))
        {
            return;
        }

        unit.Location = FitToBoard(unit, Units);

        Units.Add(unit);
        if (!Teams.Contains(unit.Team))
            Teams.Add(unit.Team);
        if (Teams.Count == 1)
            currentTeam = unit.Team;
    }

    private Coordinate FitToBoard(Unit unit, List<Unit> units)
    {
        var retval = unit.Location.Copy();

        if (!IsOnBoard(unit.Location))
        {
            if (units.Any(u => u.Team == unit.Team))
                retval = units.FirstOrDefault(u => u.Team == unit.Team)?.Location
                    ?? Coordinate.Offset(0, 0);
            else
                retval = Coordinate.Offset(0, 0);
        }

        var start = retval.Copy();
        var neighbors = retval.Neighbors();

        while (units.Any(u => u.Location == retval))
        {
            var target = retval.MoveEast(1);

            if (neighbors.Any())
            {
                target = neighbors[0];
                neighbors.RemoveAt(0);
            }

            if (!IsOnBoard(target))
            {
                target = retval.MoveSouthWest(1);
            }
            retval = target;
        }

        return retval;
    }

    public bool IsOnBoard(Coordinate loc)
    {
        return loc.Col >= 0 && loc.Row >= 0 &&
            loc.Col <= Dimension && loc.Row <= Dimension;

    }
}