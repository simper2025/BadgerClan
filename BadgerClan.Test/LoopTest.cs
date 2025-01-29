using BadgerClan.Logic;
using Xunit.Abstractions;

namespace BadgerClan.Test;

public class LoopTest
{
    ITestOutputHelper output;

    public LoopTest(ITestOutputHelper outputHelper)
    {
        output = outputHelper;
    }

    [Fact]
    public void TestName()
    {
        output.WriteLine("write out some data");
        var start = 50;
        for (var i = start; i > 0; i--)
        {
            var meds = GameEngine.CalculateMeds(i, start);
            //output.WriteLine($"For {i} players left you get {meds}.");
        }
    }
}