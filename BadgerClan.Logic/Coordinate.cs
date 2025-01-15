namespace BadgerClan.Logic;


// https://www.redblobgames.com/grids/hexagons/
/*
odd-r layout with axial underpinnings
*/
public class Coordinate
{
    public int Q;
    public int R;

    public int Col
    {
        get
        {
            return Q + (R - (R & 1)) / 2;
        }
    }
    public int Row { get { return R; } }

    public static Coordinate Offset(int col, int row)
    {
        var q = col - (row - (row & 1)) / 2;
        var r = row;
        return new Coordinate(q, r);
    }

    public Coordinate(int q, int r)
    {
        Q = q;
        R = r;
    }

    public Coordinate MoveWest(int distance) => MoveEast(-1 * distance);
    public Coordinate MoveEast(int distance)
    {
        return new Coordinate(Q + distance, R); ;
    }


    public Coordinate MoveNorthWest(int distance) => MoveSouthEast(-1 * distance);
    public Coordinate MoveSouthEast(int distance)
    {
        return new Coordinate(Q, R + distance);
    }


    public Coordinate MoveNorthEast(int distance) => MoveSouthWest(-1 * distance);
    public Coordinate MoveSouthWest(int distance)
    {
        return new Coordinate(Q - distance, R + distance);
    }


    public int Distance(Coordinate right)
    {
        var vec = Subtract(this, right);

        return (Math.Abs(vec.Q) + Math.Abs(vec.Q + vec.R) + Math.Abs(vec.R)) / 2;
    }

    private static Coordinate Subtract(Coordinate left, Coordinate right)
    {
        return new Coordinate(left.Q - right.Q, left.R - right.R);
    }
}