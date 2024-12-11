namespace HoofIt.Tests;

public class TopoMapTests
{
  private readonly string[] _exampleInput = [
    "89010123",
    "78121874",
    "87430965",
    "96549874",
    "45678903",
    "32019012",
    "01329801",
    "10456732",
  ];
  
  private async Task<string[]> GetPuzzleInput()
  {
    return await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task From_WhenCalled_ItShouldReturnNewInstanceOfTopoMap()
  {
    var result = TopoMap.From(_exampleInput);

    await Assert.That(result).IsTypeOf<TopoMap>();
  }

  [Test]
  public async Task GetTrailheadScores_WhenCalledWithExampleInput_ItShouldReturnExpectedValues()
  {
    var expectedValues = new List<int>()
    {
      5,
      6,
      5,
      3,
      1,
      3,
      5,
      3,
      5,
    };

    var result = TopoMap.From(_exampleInput).GetTrailheadScores();
    
    await Assert.That(result).IsEquivalentTo(expectedValues);
    await Assert.That(result.Sum()).IsEqualTo(36);
  }

  [Test]
  public async Task PartOne_WhenGivenPuzzleInput_ItShouldReturnExpectedValue()
  {
    var input = await GetPuzzleInput();

    var result = TopoMap.From(input).GetTrailheadScores().Sum();

    await Assert.That(result).IsEqualTo(496);
  }
  
  [Test]
  public async Task GetTrailheadRatings_WhenCalledWithExampleInput_ItShouldReturnExpectedValues()
  {
    var expectedValues = new List<int>()
    {
      20,
      24,
      10,
      4,
      1,
      4,
      5,
      8,
      5,
    };

    var result = TopoMap.From(_exampleInput).GetTrailheadRatings();
    
    await Assert.That(result).IsEquivalentTo(expectedValues);
    await Assert.That(result.Sum()).IsEqualTo(81);
  }
  
  [Test]
  public async Task PartTwo_WhenGivenPuzzleInput_ItShouldReturnExpectedValue()
  {
    var input = await GetPuzzleInput();

    var result = TopoMap.From(input).GetTrailheadRatings().Sum();

    await Assert.That(result).IsEqualTo(1120);
  }
}