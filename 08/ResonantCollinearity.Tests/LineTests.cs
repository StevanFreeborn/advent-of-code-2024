namespace ResonantCollinearity.Tests;

public class LineTests
{
  private readonly string[] _exampleInput = [
    "............",
    "........0...",
    ".....0......",
    ".......0....",
    "....0.......",
    "......A.....",
    "............",
    "............",
    "........A...",
    ".........A..",
    "............",
    "............",
  ];
  
  [Test]
  public async Task Slope_WhenCalled_ItShouldReturnExpectedValue()
  {
    var line = new Line(new(3, 4), new(5, 5), _exampleInput);

    var result = line.GetAntinodes();

    await Assert.That(result).IsEquivalentTo(new List<Point>()
    {
      new(1, 3), 
      new(7, 6),
    });
  }
}