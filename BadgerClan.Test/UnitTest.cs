
using BadgerClan.Logic;

namespace BadgerClan.Test;


public class UnitTest
{


    [Fact]
    public void KnightTest(){
        var knight = "Knight";
        var unit = Unit.Factory(knight, 1, Coordinate.Offset(1,1));
        Assert.Equal(knight, unit.Type);
    }

    [Fact]
    public void DefaultFactoryTest(){
        var unit = Unit.Factory("Unknown", 1, Coordinate.Offset(1,1));
        Assert.Equal("Peasant", unit.Type);        
    }
}