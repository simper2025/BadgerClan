
namespace BadgerClan.Logic;


public class GameState
{
    public int Dimension = 70;

    public List<Unit> Units { get; set; }
    private List<int> TurnOrder;

    private List<Team> TeamList;

    public int TeamCount { get { return TeamList.Count(); } }

    public int Turn { get; private set; }

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

    public GameState()
    {
        Units = new List<Unit>();
        TurnOrder = new List<int>();
        TeamList = new List<Team>();
        Turn = 0;
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
    public void AddTeam(int team, Coordinate loc, List<string> units)
    {
        if (!TeamList.Any(t => t.Id == team))
        {
            AddTeam(new Team(team));
        }
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
        if (!TeamList.Any(t => t.Id == unit.Team))
        // if (Turn > 0 && !Units.Any(u => u.Team == unit.Team))
        {
            return;
        }

        unit.Location = FitToBoard(unit, Units);

        Units.Add(unit);

        // Remove this soon
        // if (!TurnOrder.Contains(unit.Team))
        //     TurnOrder.Add(unit.Team);
        // if (!TeamList.Any(t => t.Id == unit.Team))
        // {
        //     TeamList.Add(new Team(unit.Team));
        // }

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