
using BadgerClab.Logic;

namespace BadgerClan.Test;


public class UnitTest
{


    [Fact]
    public void KnightTest(){
        var knight = "Knight";
        var unit = Unit.Factory(knight, new Coordinate(1,1));
        Assert.Equal(knight, unit.Type);
    }
}