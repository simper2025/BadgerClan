namespace BadgerClan.Logic;

public class Unit
{
    protected static int Next_Id = 1;
    public int Id { get; private set; }
    public int Team;

    public string Type { get; set; }
    public Coordinate Location { get; set; }
    public double MaxMoves { get; set; }
    public double Moves { get; set; }
    public int Attack { get; set; }
    public int AttackDistance { get; set; }
    public int Health { get; set; }

    public static Unit Factory(string name, int team)
    {
        return Factory(name, team, new Coordinate(-1, -1));
    }
    public static Unit Factory(string name, Coordinate loc)
    {
        return Factory(name, 0, loc);
    }
    public static Unit Factory(string name, int team, Coordinate loc)
    {
        var unit = new Unit
        {
            Location = loc,
            Attack = 1,
            Health = 5,
            MaxMoves = 1,
            AttackDistance = 1,
            Moves = 1,
            Team = team,
        };

        switch (name)
        {
            case "Knight":
                unit.Type = "Knight";
                unit.Attack = 4;
                unit.Health = 10;
                unit.MaxMoves = 2;
                unit.Moves = 2;
                break;
            case "Archer":
                unit.Type = "Archer";
                unit.Attack = 2;
                unit.Health = 9;
                unit.MaxMoves = 3;
                unit.AttackDistance = 3;
                unit.Moves = 3;
                break;
        }

        return unit;
    }

    protected Unit()
    {
        Id = Next_Id++;
        Type = "Peasent";
        Location = Coordinate.Offset(0, 0);
    }
}