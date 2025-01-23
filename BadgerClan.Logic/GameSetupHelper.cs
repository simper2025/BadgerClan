namespace BadgerClan.Logic;

public class GameSetupHelper
{
    public static Coordinate GetCircleCoordinate(int deg, int size)
    {
        var radius = size / 2 - 10;
        var center = Coordinate.Offset(size / 2, size / 2);

        double radians = (deg - 90) * (Math.PI / 180);

        double x = radius * Math.Cos(radians);
        double y = radius * Math.Sin(radians);

        var loc = center + Coordinate.Offset((int)x, (int)y);
        return loc;
    }

}