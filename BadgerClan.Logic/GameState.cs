
namespace BadgerClan.Logic;


public class GameState
{
    public DateTime Created { get; } = DateTime.Now;
    public Guid Id { get; } = Guid.NewGuid();
    public event Action<GameState> GameChanged;
    private static int NextId = 1;
    public string Name { get; set; }
    public int Dimension = 70;

    public List<Unit> Units { get; set; }
    private List<int> teams;
    private Dictionary<int, string> teamNames = new();

    public int TeamCount { get { return teams.Count(); } }

    public IEnumerable<string> TeamNames => teamNames.Values;

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
        teams = new List<int>();
        Turn = 0;
        Name = name ?? $"Game{NextId++}";
    }

    public override string ToString()
    {
        string status = "Turn #" + Turn + "; ";
        if (Running)
        {
            foreach (int team in teams)
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
        var teamIndex = teams.IndexOf(currentTeam);
        if (teamIndex != teams.Count - 1)
            teamIndex++;
        else
            teamIndex = 0;
        currentTeam = teams[teamIndex];

        Turn++;
    }

    public void AddTeam(int team, string teamName, Coordinate loc, List<string> units)
    {
        teamNames[team] = teamName;
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
        if (Turn > 0 && !Units.Any(u => u.Team == unit.Team))
        {
            return;
        }

        unit.Location = FitToBoard(unit, Units);

        Units.Add(unit);
        if (!teams.Contains(unit.Team))
            teams.Add(unit.Team);
        if (teams.Count == 1)
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