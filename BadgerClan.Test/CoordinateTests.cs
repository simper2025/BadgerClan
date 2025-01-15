
using BadgerClab.Logic;

namespace BadgerClan.Test;

public class CoordinateTests
{

    [Theory]
    [InlineData(2,2,3,2,1)]
    [InlineData(1,1,3,1,2)]
    [InlineData(1,1,5,4,5)]
    public void Distance(int a1, int a2, int b1, int b2, int expected)
    {
        var p1 = new Coordinate(a1, a2);
        var p2 = new Coordinate(b1, b2);
        var distance = p1.Distance(p2);
        Assert.Equal(expected, distance);
    }
}