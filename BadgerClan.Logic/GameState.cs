
namespace BadgerClan.Logic;


public class GameState
{
    public int Dimension = 100;

    public List<Unit> Units { get; set; }
    private List<int> Teams;

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

    public GameState()
    {
        Units = new List<Unit>();
        Teams = new List<int>();
        Turn = 0;
    }

    public override string ToString()
    {
        string status = "";
        if(Running)
        {
            foreach(int team in Teams){
                status += "Team " + team +": " + Units.Count(u => u.Team == team) + "; ";
            }
        }
        else if (Turn > 0)
        {
            status = "GameOver; Team #" + Units.FirstOrDefault().Team + " wins";
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
        if (!Units.Contains(unit))
        {
            if (Turn > 0 && !Units.Any(u => u.Team == unit.Team))
            {
                return;
            }
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
            if (!Teams.Contains(unit.Team))
                Teams.Add(unit.Team);
            if (Teams.Count == 1)
                currentTeam = unit.Team;
        }
    }

    public bool IsOnBoard(Coordinate loc)
    {
        return loc.Col >= 0 && loc.Row >= 0 &&
            loc.Col <= Dimension && loc.Row <= Dimension;

    }
}