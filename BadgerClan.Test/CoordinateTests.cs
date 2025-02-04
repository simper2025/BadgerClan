
using System.Security.AccessControl;
using BadgerClan.Logic;

namespace BadgerClan.Test;

public class CoordinateTests
{

    [Theory]
    [InlineData(2, 2, 3, 2, 1)]
    [InlineData(1, 1, 3, 1, 2)]
    [InlineData(1, 1, 5, 4, 5)]
    public void Distance(int a1, int a2, int b1, int b2, int expected)
    {
        var p1 = Coordinate.Offset(a1, a2);
        var p2 = Coordinate.Offset(b1, b2);
        var distance = p1.Distance(p2);
        Assert.Equal(expected, distance);
    }

    [Fact]
    public void MoveEast()
    {
        var p1 = Coordinate.Offset(3, 3);
        var result = p1.MoveEast(1);
        Assert.Equal(4, result.Col);
        Assert.Equal(3, result.Row);
    }

    [Fact]
    public void MoveWest()
    {
        var p1 = Coordinate.Offset(3, 3);
        var result = p1.MoveWest(1);
        Assert.Equal(2, result.Col);
        Assert.Equal(3, result.Row);
    }

    [Fact]
    public void MoveSouthEast()
    {
        var p1 = Coordinate.Offset(3, 3);
        var result = p1.MoveSouthEast(1);
        Assert.Equal(4, result.Col);
        Assert.Equal(4, result.Row);
    }

    [Fact]
    public void MoveNorthWest()
    {
        var p1 = Coordinate.Offset(3, 3);
        var result = p1.MoveNorthWest(1);
        Assert.Equal(3, result.Col);
        Assert.Equal(2, result.Row);
    }

    [Fact]
    public void MoveSouthWest()
    {
        var p1 = Coordinate.Offset(3, 3);
        Assert.Equal(2, p1.Q);
        Assert.Equal(3, p1.R);
        var result = p1.MoveSouthWest(1);
        Assert.Equal(1, result.Q); //-1
        Assert.Equal(4, result.R); //4

        Assert.Equal(3, result.Col);
        Assert.Equal(4, result.Row);
    }

    [Fact]
    public void MoveNorthEast()
    {
        var p1 = Coordinate.Offset(3, 3);
        var result = p1.MoveNorthEast(1);
        Assert.Equal(4, result.Col);
        Assert.Equal(2, result.Row);
    }

    [Fact]
    public void AreEqualTest()
    {
        var p1 = new Coordinate(5, 12);
        var p2 = new Coordinate(5, 12);
        Assert.Equal(p1, p2);
    }

    [Fact]
    public void GetNeighbors()
    {
        var p1 = Coordinate.Offset(4, 4);
        var neighbors = p1.Neighbors();
        Assert.Equal(6, neighbors.Count);

        Assert.Contains(Coordinate.Offset(3, 3), neighbors);
        Assert.Contains(Coordinate.Offset(4, 3), neighbors);
        Assert.Contains(Coordinate.Offset(5, 4), neighbors);
        Assert.Contains(Coordinate.Offset(4, 5), neighbors);
        Assert.Contains(Coordinate.Offset(3, 5), neighbors);
        Assert.Contains(Coordinate.Offset(3, 4), neighbors);
    }

    [Fact]
    public void GetNeighborsTwo()
    {
        var p1 = Coordinate.Offset(4, 4);
        var neighbors = p1.Neighbors(2);
        Assert.Equal(12, neighbors.Count);

        Assert.Contains(Coordinate.Offset(3, 2), neighbors);
        Assert.Contains(Coordinate.Offset(4, 2), neighbors);
        Assert.Contains(Coordinate.Offset(5, 2), neighbors);
        Assert.Contains(Coordinate.Offset(5, 3), neighbors);
        Assert.Contains(Coordinate.Offset(6, 4), neighbors);
        Assert.Contains(Coordinate.Offset(5, 5), neighbors);

        Assert.Contains(Coordinate.Offset(5, 6), neighbors);
        Assert.Contains(Coordinate.Offset(4, 6), neighbors);
        Assert.Contains(Coordinate.Offset(3, 6), neighbors);
        Assert.Contains(Coordinate.Offset(2, 5), neighbors);
        Assert.Contains(Coordinate.Offset(2, 4), neighbors);
        Assert.Contains(Coordinate.Offset(2, 3), neighbors);

    }

    [Fact]
    public void TowardTest()
    {
        var point = Coordinate.Offset(1, 1);
        var toward = point.Toward(Coordinate.Offset(3, 4));
        var expected = Coordinate.Offset(2, 2);
        Assert.Equal(expected, toward);
    }

    [Fact]
    public void AwayTest()
    {
        var point = Coordinate.Offset(3, 4);
        var away = point.Away(Coordinate.Offset(1, 1));
        var expected = Coordinate.Offset(3, 5);
        Assert.Equal(expected, away);
    }

    [Theory]
    [InlineData(1, 0, 4, 3)]
    [InlineData(0, -1, 3, 2)]
    [InlineData(-1, 1, 3, 4)]
    public void AddTest(int q, int r, int col, int row)
    {
        var start = Coordinate.Offset(3, 3);
        var vector = new Coordinate(q, r);
        var expected = Coordinate.Offset(col, row);

        var result = start + vector;

        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(-1, 0, 4, 3)]
    [InlineData(0, 1, 3, 2)]
    [InlineData(1, -1, 3, 4)]
    public void SubtractTest(int q, int r, int col, int row)
    {
        var p1 = Coordinate.Offset(3, 3);
        var p2 = Coordinate.Offset(col, row);
        var expected = new Coordinate(q, r);

        var result = p1 - p2;

        Assert.Equal(expected, result);
    }

}