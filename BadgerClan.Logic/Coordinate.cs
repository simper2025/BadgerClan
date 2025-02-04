using System.Text.Json.Serialization;

namespace BadgerClan.Logic;


// https://www.redblobgames.com/grids/hexagons/
/*
odd-r layout with axial underpinnings
*/
public class Coordinate : IEquatable<Coordinate>
{
    public int Q { get; set; }
    public int R { get; set; }

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

    [JsonConstructor]
    public Coordinate(int Q, int R)
    {
        this.Q = Q;
        this.R = R;
    }


    #region object methods
    public override string ToString()
    {
        return $"Coordinate {{ Q = {Q}, R = {R}, Col = {Col}, Row = {Row} }}";
    }

    public Coordinate Copy()
    {
        return new Coordinate(Q, R);
    }

    public bool Equals(Coordinate? other)
    {
        if (other is null) return false;
        return Q == other.Q && R == other.R;
    }
    public override bool Equals(object? obj) => Equals(obj as Coordinate);

    public static bool operator ==(Coordinate left, Coordinate right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null) return false;
        if (right is null) return false;
        return left.Equals(right);
    }
    public static bool operator !=(Coordinate left, Coordinate right) => !(left == right);

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Q.GetHashCode();
            hashCode = (hashCode * 397) ^ R.GetHashCode();
            return hashCode;
        }
    }

    public static Coordinate operator +(Coordinate left, Coordinate right)
    {
        return new Coordinate(left.Q + right.Q, left.R + right.R);
    }
    public static Coordinate operator -(Coordinate left, Coordinate right)
    {
        return new Coordinate(left.Q - right.Q, left.R - right.R);
    }
    #endregion

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


    public List<Coordinate> Neighbors(int distance = 1)
    {
        var neighbors = new List<Coordinate>();

        for (var q = -distance; q <= distance; q++)
        {
            for (var r = -distance; r <= distance; r++)
            {
                var tempLoc = new Coordinate(Q + q, R + r);
                if (Distance(tempLoc) == distance)
                    neighbors.Add(tempLoc);
            }
        }

        return neighbors;
    }

    public Coordinate Toward(Coordinate end)
    {
        var r = 0;
        if (end.R - R < 0)
            r = -1;
        else if (end.R - R > 0)
            r = +1;

        var q = 0;
        if (end.Q - Q < 0)
            q = -1;
        else if (end.Q - Q > 0)
            q = +1;

        var target = new Coordinate(Q + q, R + r);
        return target;
    }

    public Coordinate Away(Coordinate end)
    {
        var r = 0;
        if (end.R - R < 0)
            r = +1;
        else if (end.R - R > 0)
            r = -1;

        var q = 0;
        if (end.Q - Q < 0)
            q = +1;
        else if (end.Q - Q > 0)
            q = -1;

        var target = new Coordinate(Q + q, R + r);
        return target;
    }

}