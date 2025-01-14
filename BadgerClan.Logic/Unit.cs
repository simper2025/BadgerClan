

using System.Drawing;

public class Unit{
    protected static int Next_Id = 1;
    public int Id { get; private set;}
    public Coordinate Location {get;set;}

    public Unit(int col, int row)
    {
        Location = new Coordinate(col, row);
        Id = Next_Id++;
    }
}