
namespace BadgerClan.Logic;


public class GameState
{
    public DateTime Created { get; } = DateTime.Now;
    public Guid Id { get; } = Guid.NewGuid();
    public event Action<GameState> GameChanged;
    public string Name { get; set; }

    public int Dimension = 70;

    public int TotalUnits = 0;
    public int Turn { get; private set; }
    public List<Unit> Units { get; set; }

    public List<Team> TeamList { get; private set; }
    private List<int> TurnOrder;

    private Dictionary<int, string> teamNames = new(); 
    public int TeamCount { get { return TeamList.Count(); } }
    public IEnumerable<string> TeamNames => teamNames.Values;

    private int currentTeam = 0;
    public int CurrentTeam
    {
        get
        {
            if (currentTeam == 0 && TurnOrder.Count > 0)
            {
                currentTeam = TurnOrder[0];
            }
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
        TurnOrder = new List<int>();
        TeamList = new List<Team>();
        Turn = 0;
        Name = name ?? $"Game{Id.ToString().Substring(32)}";
    }

    public override string ToString()
    {
        string status = "Turn #" + Turn + "; ";
        if (Running)
        {
            foreach (int team in TurnOrder)
            {
                status += "Team " + team + ": " + Units.Count(u => u.Team == team) + "; ";
            }
        }
        else if (Turn > 0)
        {
            var team = Units.FirstOrDefault()?.Team ?? 0;
            status = "GameOver; Team #" + team + " wins";
        }
        status += " Medpacs" + TeamList.Sum(t => t.Medpacs);
        return status;
    }

    public void IncrementTurn()
    {
        currentTeam = AdvanceTeam();

        Turn++;
    }

    private int AdvanceTeam()
    {
        var teamIndex = TurnOrder.IndexOf(currentTeam);
        // possibly change
        if (teamIndex < 0)
            return 0;

        if (teamIndex != TurnOrder.Count - 1)
            teamIndex++;
        else
            teamIndex = 0;
        return TurnOrder[teamIndex];
    }

    public void AddTeam(Team team)
    {
        TeamList.Add(team);
        TurnOrder.Add(team.Id);
    }

    public void StartGame(List<string> units)
    {
        var degrees = 360 / TeamList.Count;
        int i = 0;

        foreach (var team in TeamList)
        {
            var loc = GetCircleCoordinate(degrees * i, Dimension);
            AddUnits(team.Id, loc, units);
            i++;
        }
        GameChanged?.Invoke(this);
    }

    public static Coordinate GetCircleCoordinate(int deg, int size)
    {
        var radius = size / 2 - 10;
        var center = Coordinate.Offset(size / 2, size / 2);

        double radians = (deg-90) * (Math.PI / 180);

        double x = radius * Math.Cos(radians);
        double y = radius * Math.Sin(radians);

        var loc = center + Coordinate.Offset((int)x, (int)y);
        return loc;
    }

    public void AddUnits(int team, Coordinate loc, List<string> units)
    {
        foreach (var unit in units)
        {
            AddUnit(Unit.Factory(unit, team, loc));
        }
        GameChanged?.Invoke(this);
    }

    public void AddUnit(Unit unit)
    {
        if (Units.Contains(unit))
        {
            return;
        }
        if (!TeamList.Any(t => t.Id == unit.Team))
        {
            return;
        }

        unit.Location = FitToBoard(unit, Units);

        Units.Add(unit);
        TotalUnits++;
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