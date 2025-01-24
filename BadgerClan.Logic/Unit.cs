
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

    public int AttackCount;
    public int Attack { get; set; }
    public int AttackDistance { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; private set; }

    public static Unit Factory(string name, int team)
    {
        return Factory(name, team, new Coordinate(-1, -1));
    }
    public static Unit Factory(string name, int team, Coordinate loc)
    {
        var unit = new Unit
        {
            Location = loc,
            Attack = 1,
            Health = 5,
            MaxHealth = 5,
            MaxMoves = 1,
            AttackDistance = 1,
            Moves = 1,
            Team = team,
            AttackCount = 1
        };

        switch (name)
        {
            case "Knight":
                unit.Type = "Knight";
                unit.Attack = 4;
                unit.Health = 15;
                unit.MaxHealth = 15;
                unit.MaxMoves = 2;
                unit.Moves = 2;
                unit.AttackCount = 2;
                break;
            case "Archer":
                unit.Type = "Archer";
                unit.Attack = 2;
                unit.Health = 9;
                unit.MaxHealth = 9;
                unit.MaxMoves = 3;
                unit.AttackDistance = 3;
                unit.Moves = 3;
                unit.AttackCount = 2;
                break;
        }

        return unit;
    }

    public static Unit Factory(string type, int id, int attack, int attackDistance, int health, int maxHealth, double moves, double maxMoves, Coordinate location, int team)
    {
        var unit = new Unit
        {
            Type = type,
            Id = id,
            Attack = attack,
            AttackDistance = attackDistance,
            Health = health,
            MaxHealth = maxHealth,
            Moves = moves,
            MaxMoves = maxMoves,
            Location = location,
            Team = team
        };
        return unit;
    }

    protected Unit()
    {
        Id = Next_Id++;
        Type = "Peasant";
        Location = Coordinate.Offset(0, 0);
    }
}