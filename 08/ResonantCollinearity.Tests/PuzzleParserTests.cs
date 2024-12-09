namespace ResonantCollinearity.Tests;

public class PuzzleParserTests
{
  private readonly PuzzleParser _parser = new();
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
  public async Task Parse_WhenGivenExampleInput_ItShouldReturnExpectedListOfFrequencies()
  {
    var antenna = new List<Antenna>()
    {
      new(1, 8, '0'),
      new(2, 5, '0'),
      new(3, 7, '0'),
      new(4, 4, '0'),
      new(5, 6, 'A'),
      new(8, 8, 'A'),
      new(9, 9, 'A'),
    };

    var expectedGroupings = antenna.GroupBy(c => c.Frequency).ToList();
    
    var result = _parser.Parse(_exampleInput);
    
    await Assert.That(result).IsEquivalentTo(expectedGroupings);
  }
  
  [Test]
  public async Task SolutionPartOne_WhenGivenExampleInput_ItShouldReturnExpectedAntinodes()
  {
    var expectedAntinodes = new List<Point>()
    {
      new(0, 11),
      new(3, 2),
      new(5, 6),
      new(7, 0),
      new(1, 3),
      new(4, 9),
      new(0, 6),
      new(6, 3),
      new(2, 10),
      new(5, 1),
      new(2, 4),
      new(11, 10),
      new(7, 7),
      new(10, 10),
    };
    
    var result = _parser.Parse(_exampleInput).GetAntinodes(_exampleInput);
    
    await Assert.That(result).IsEquivalentTo(expectedAntinodes);
  }
  
  [Test]
  public async Task SolutionPartTwo_WhenGivenExampleInput_ItShouldReturnExpectedAntinodes()
  {
    // var expectedAntinodes = new List<Point>()
    // {
    //   new(0, 11),
    //   new(3, 2),
    //   new(5, 6),
    //   new(7, 0),
    //   new(1, 3),
    //   new(4, 9),
    //   new(0, 6),
    //   new(6, 3),
    //   new(2, 10),
    //   new(5, 1),
    //   new(2, 4),
    //   new(11, 10),
    //   new(7, 7),
    //   new(10, 10),
    // };
    
    var result = _parser.Parse(_exampleInput).GetAntinodes(_exampleInput, isPart2: true);
    
    await Assert.That(result.Count).IsEquivalentTo(34);
  }
}