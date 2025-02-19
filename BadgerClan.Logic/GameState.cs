namespace BadgerClan.Logic;

public class GameState
{
    public DateTime Created { get; } = DateTime.Now;
    public Guid Id { get; } = Guid.NewGuid();
    public event Action<GameState>? GameChanged;
    public event Action<GameState>? GameEnded;
    public string Name { get; set; }

    public int Dimension = 70;

    public int TotalUnits = 0;
    public int TurnNumber { get; private set; }
    public List<Unit> Units { get; set; }

    public List<Team> TeamList { get; init; }
    private List<int> turnOrder;

    public int TeamCount { get { return TeamList.Count(); } }
    public IEnumerable<string> TeamNames => TeamList.Select(t => t.Name);

    public List<GameLog> Logs { get; private set; } = [];
    private bool isGameOver;
    public bool IsGameOver
    {
        get => isGameOver;
        set
        {
            isGameOver = value;
            GameEnded?.Invoke(this);
        }
    }

    public Team GetWinner()
    {
        if (IsGameOver)
        {
            return TeamList.FirstOrDefault(t => t.Id == Units.Select(u => u.Team).First());
        }
        return new Team(-1);
    }
    public DateTime LastMove { get; set; } = DateTime.Now;

    private int currentTeamId = 0;
    public int CurrentTeamId
    {
        get
        {
            if (currentTeamId == 0 && turnOrder.Count > 0)
            {
                currentTeamId = turnOrder[0];
            }
            return currentTeamId;
        }
    }

    public Team CurrentTeam => TeamList.First(t => t.Id == CurrentTeamId);

    public bool Running
    {
        get
        {
            if (TurnNumber == 0)
                return false;
            return Units.Select(u => u.Team).Distinct().Count() > 1;
        }
    }

    public int NextMedpac
    {
        get
        {
            return GameEngine.CalculateMeds(Units.Count, TotalUnits);
        }
    }

    public GameState(string? name = null)
    {
        Units = new List<Unit>();
        turnOrder = new List<int>();
        TeamList = new List<Team>();
        TurnNumber = 0;
        Name = name ?? $"Game-{Id.ToString().Substring(32)}";
    }

    public GameState(string gameId, int boardSize, int turnNumber, IEnumerable<Unit>? units, IEnumerable<int> teamIds, Team currentTeam)
    {
        Id = Guid.Parse(gameId);
        Dimension = boardSize;
        TurnNumber = turnNumber;
        Units = units?.ToList() ?? new List<Unit>();
        TeamList = [currentTeam];
        turnOrder = [currentTeam.Id];
        Name = $"Game-{Id.ToString().Substring(32)}";
    }

    public void RestartGame() {
        Units = new List<Unit>();
        TotalUnits = 0;
        TurnNumber = 0;
        foreach (var team in TeamList)
        {
            team.Medpacs = 0;
        }
        currentTeamId = TeamList[0].Id;
    }

    public override string ToString()
    {
        string status = "Turn #" + TurnNumber + "; ";
        if (Running)
        {
            foreach (Team team in TeamList)
            {
                var unitCount = Units.Count(u => u.Team == team.Id);
                status += $"{team.Name}: {unitCount}; ";
            }
        }
        else if (TurnNumber > 0)
        {
            var teamid = Units.FirstOrDefault()?.Team ?? 0;
            var team = TeamList.FirstOrDefault(team => team.Id == teamid)?.Name ?? "empty";
            status = $"GameOver: {team} wins";
        }
        //status += " Medpacs" + TeamList.Sum(t => t.Medpacs);
        return status;
    }

    public void IncrementTurn()
    {
        do
        {
            currentTeamId = AdvanceTeam();
        } while (Units.Count > 0 && !Units.Any(u => u.Team == currentTeamId));
        TurnNumber++;
        GameChanged?.Invoke(this);
        LastMove = DateTime.Now;

        IsGameOver = Units.Select(u => u.Team).Distinct().Count() == 1;
    }

    private int AdvanceTeam()
    {
        var teamIndex = turnOrder.IndexOf(currentTeamId);
        // possibly change
        if (teamIndex < 0)
            return 0;

        if (teamIndex != turnOrder.Count - 1)
            teamIndex++;
        else
            teamIndex = 0;
        return turnOrder[teamIndex];
    }

    public void AddTeam(Team team)
    {
        TeamList.Add(team);
        turnOrder.Add(team.Id);
        GameChanged?.Invoke(this);
    }

    public void LayoutStartingPositions(List<string> units)
    {
        var degrees = 360 / TeamList.Count;
        int i = 0;

        foreach (var team in TeamList)
        {
            var loc = GameSetupHelper.GetCircleCoordinate(degrees * i, Dimension);
            AddUnits(team.Id, loc, units);
            i++;
        }
        GameChanged?.Invoke(this);
    }

    public void AddUnits(int team, Coordinate location, List<string> units)
    {
        foreach (var unit in units)
        {
            AddUnit(Unit.Factory(unit, team, location));
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
        var retval =  unit.Location.Copy();
        if(!IsOnBoard(retval))
            retval = teamMateLocationOrZero(unit, units);

        var start = retval;
        var neighbors = new List<Coordinate>();
        var neighborDistance = 1;

        while (units.Any(u => u.Location == retval) || !IsOnBoard(retval))
        {
            if (!neighbors.Any())
            {
                neighbors = start.Neighbors(neighborDistance++);
            }
            retval = neighbors[0];
            neighbors.RemoveAt(0);
        }

        return retval;
    }

    private Coordinate teamMateLocationOrZero(Unit unit, List<Unit> units)
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
        return retval;
    }

    public bool IsOnBoard(Coordinate location)
    {
        return location.Col >= 0 && location.Row >= 0 &&
            location.Col <= Dimension && location.Row <= Dimension;

    }
}