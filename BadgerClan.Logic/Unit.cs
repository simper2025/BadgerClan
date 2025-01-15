

using System.Drawing;

public class Unit
{
    protected static int Next_Id = 1;
    public int Id { get; private set; }

    public string Type { get; set; }
    public Coordinate Location { get; set; }
    public int Movement { get; set; }
    public int Attack { get; set; }
    public int Health { get; set; }

    public static Unit Factory(string name, Coordinate loc)
    {
        var unit = new Unit
        {
            Location = loc,
            Movement = 1,
            Attack = 1,
            Health = 5,
        };

        switch (name)
        {
            case "Knight":
                unit.Type = "Knight";
                unit.Attack = 4;
                unit.Movement = 2;
                unit.Health = 10;
                break;
        }

        return unit;
    }

    protected Unit()
    {
        Id = Next_Id++;
        Type = "Peasent";
        Location = new Coordinate(0,0);
    }
}