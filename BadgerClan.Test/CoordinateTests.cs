
using BadgerClab.Logic;

namespace BadgerClan.Test;

public class CoordinateTests
{

    [Fact]
    public void Distance()
    {
        var p1 = new Coordinate(2, 2);
        var p2 = new Coordinate(3, 2);
        var distance = p1.Distance(p2);
        Assert.Equal(1, distance);
    }
}