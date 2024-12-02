namespace HistorianHysteria.Tests;

public class PuzzleSolverTests
{
  private readonly PuzzleParser _puzzleParser = new();
  private readonly PuzzleSolver _puzzleSolver = new();
  private readonly List<int> _leftTestList = [3, 4, 2, 1, 3, 3];
  private readonly List<int> _rightTestList = [4, 3, 5, 3, 9, 3];

  private async Task<(List<int> left, List<int> Right)> GetPuzzleLists()
  {
    var inputPath = Path.Combine(AppContext.BaseDirectory, "INPUT.txt");
    var input = await File.ReadAllLinesAsync(inputPath);
    return _puzzleParser.Parse(input);
  }

  [Test]
  public async Task CalculateTotalDistance_WhenGivenLists_ItShouldReturnTotalDistance()
  {
    var result = _puzzleSolver.CalculateTotalDistance(_leftTestList, _rightTestList);

    await Assert.That(result).IsEqualTo(11);
  }

  [Test]
  public async Task CalculateTotalDistance_WhenGivenPuzzleInputForPartOne_ItShouldReturnTotalDistance()
  {
    var (left, right) = await GetPuzzleLists();

    var result = _puzzleSolver.CalculateTotalDistance(left, right);

    await Assert.That(result).IsEqualTo(1765812);
  }

  [Test]
  public async Task CalculateSimilarityScore_WhenGivenLists_ItShouldReturnSimilarityScore()
  {
    var result = _puzzleSolver.CalculateSimilarityScore(_leftTestList, _rightTestList);

    await Assert.That(result).IsEqualTo(31);
  }

  [Test]
  public async Task CalculateSimilarityScore_WhenGivenPuzzleInputForPartOne_ItShouldReturnSimilarityScore()
  {
    var (left, right) = await GetPuzzleLists();
    var result = _puzzleSolver.CalculateSimilarityScore(left, right);

    await Assert.That(result).IsEqualTo(20520794);
  }
}
