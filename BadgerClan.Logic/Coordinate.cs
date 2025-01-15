

// https://www.redblobgames.com/grids/hexagons/
/*
odd-r layout with axial underpinnings
*/
public class Coordinate
{
    private int q;
    private int r;

    public int Col
    {
        get
        {
            return q + (r - (r & 1)) / 2;
        }
    }
    public int Row { get { return r; } }

    public Coordinate(int col, int row)
    {
        q = col - (row - (row & 1)) / 2;
        r = row;

    }

    private Coordinate(int qq, int rr, bool axial)
    {
        if (axial)
        {
            q = qq;
            r = rr;
        }
        else
        {
            q = qq - (rr - (qq & 1)) / 2;
            r = rr;
        }
    }

    public Coordinate MoveEast(int distance)
    {
        return new Coordinate(q + distance, r, true); ;
    }

    public Coordinate MoveSouthEast(int distance)
    {
        return new Coordinate(q, r + distance, true);
    }

    public Coordinate MoveSouthWest(int distance)
    {
        return new Coordinate(q - 1, r + 1);
    }


    public int Distance(Coordinate right)
    {
        var vec = Subtract(this, right);

        return (Math.Abs(vec.q) + Math.Abs(vec.q + vec.r) + Math.Abs(vec.r)) / 2;
    }

    private static Coordinate Subtract(Coordinate left, Coordinate right)
    {
        return new Coordinate(left.q - right.q, left.r - right.r, true);
    }
}