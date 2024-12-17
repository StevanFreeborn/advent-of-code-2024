namespace WarehouseWoes.Tests;

public class PuzzleParserTests
{
  private async Task<string> GetPuzzleInput()
  {
    return await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task Parse_WhenCalled_ItShouldReturnMapAndDirections()
  {
    var input = await GetPuzzleInput();
    var result = PuzzleParser.Parse(input);

    await Assert.That(result.Map).IsTypeOf<WarehouseMap>();
    await Assert.That(result.Directions).IsTypeOf<List<Direction>>();
  }

  [Test]
  public async Task PartOneSolution_WhenGivenPuzzleInput_ItShouldReturnExpectedSolution()
  {
    var input = await GetPuzzleInput();
    var (map, directions) = PuzzleParser.Parse(input);

    var result = map.MoveRobot(directions).CalculateTotalBoxGpsCoordinates();

    await Assert.That(result).IsEqualTo(1318523);
  }
}