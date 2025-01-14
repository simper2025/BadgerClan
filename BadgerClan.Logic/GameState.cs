

public class GameState
{
    public List<Unit> Units {get;set;}

    public GameState()
    {
        Units = new List<Unit>();
    }

    public void AddUnit(Unit unit){
        Units.Add(unit);
    }
}