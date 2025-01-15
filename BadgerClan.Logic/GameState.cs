namespace BadgerClan.Logic;


public class GameState
{
    public int Dimension = 100;

    public List<Unit> Units {get;set;}

    public GameState()
    {
        Units = new List<Unit>();
    }

    public void AddUnit(Unit unit){
        Units.Add(unit);
    }
}