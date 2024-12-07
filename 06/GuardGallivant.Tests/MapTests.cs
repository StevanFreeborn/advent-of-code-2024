namespace GuardGallivant.Tests;

public class MapTests
{
  private readonly string[] _exampleMap = [
    "....#.....",
    ".........#",
    "..........",
    "..#.......",
    ".......#..",
    "..........",
    ".#..^.....",
    "........#.",
    "#.........",
    "......#...",
  ];

  private async Task<string[]> GetPuzzleInput()
  {
    return await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task PredictDistinctPositionsCount_WhenCalledWithExample_ItShouldReturnExpectedCount()
  {
    var map = new Map(_exampleMap);
  
    var result = map.PredictDistinctPositionsCount();
    
    await Assert.That(result).IsEqualTo(41);
  }
  
  [Test]
  public async Task PredictDistinctPositionsCount_WhenCalledWithPuzzleInput_ItShouldReturnExpectedCount()
  {
    var input = await GetPuzzleInput();
    var map = new Map(input);
  
    var result = map.PredictDistinctPositionsCount();
    
    await Assert.That(result).IsEqualTo(4789);
  }
  
  [Test]
  public async Task IdentifyNumberOfPlacementsForNewObstruction_WhenCalledWithExample_ItShouldReturnExpectedCount()
  {
    var map = new Map(_exampleMap);
  
    var result = map.IdentifyNumberOfPlacementsForNewObstruction();
    
    await Assert.That(result).IsEqualTo(6);
  }
  
  [Test]
  public async Task IdentifyNumberOfPlacementsForNewObstruction_WhenCalledWithPuzzleInput_ItShouldReturnExpectedCount()
  {
    var input = await GetPuzzleInput();
    var map = new Map(input);
  
    var result = map.IdentifyNumberOfPlacementsForNewObstruction();
    
    await Assert.That(result).IsEqualTo(1304);
  }
}