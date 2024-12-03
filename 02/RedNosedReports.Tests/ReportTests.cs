namespace RedNosedReports.Tests;

public class ReportTests
{
  private readonly PuzzleParser _puzzleParser = new();

  private async Task<List<Report>> GetPuzzleLists()
  {
    var inputPath = Path.Combine(AppContext.BaseDirectory, "INPUT.txt");
    var input = await File.ReadAllLinesAsync(inputPath);
    return _puzzleParser.Parse(input);
  }

  [Test]
  public async Task IsSafe_WhenGivenPuzzleInput_ItShouldReturnExpectedCount()
  {
    var reports = await GetPuzzleLists();
    var count = reports.Count(r => r.IsSafe());

    await Assert.That(count).IsEqualTo(407);
  }

  [Test]
  public async Task IsSafe_WhenAllLevelsAreNotDecreasing_ItShouldReturnFalse()
  {
    var report = new Report([7, 6, 9, 2, 1]);

    await Assert.That(report.IsSafe()).IsEqualTo(false);
  }

  [Test]
  public async Task IsSafe_WhenAllLevelsAreDecreasing_ItShouldReturnTrue()
  {
    var report = new Report([7, 6, 4, 2, 1]);

    await Assert.That(report.IsSafe()).IsEqualTo(true);
  }

  [Test]
  public async Task IsSafe_WhenAllLevelsAreNotIncreasing_ItShouldReturnFalse()
  {
    var report = new Report([1, 3, 6, 4, 9]);

    await Assert.That(report.IsSafe()).IsEqualTo(false);
  }

  [Test]
  public async Task IsSafe_WhenAllLevelsAreIncreasing_ItShouldReturnTrue()
  {
    var report = new Report([1, 3, 6, 7, 9]);

    await Assert.That(report.IsSafe()).IsEqualTo(true);
  }

  [Test]
  public async Task IsSafe_WhenTheChangeIsTooLarge_ItShouldReturnFalse()
  {
    var report = new Report([1, 2, 7, 8, 9]);

    await Assert.That(report.IsSafe()).IsEqualTo(false);
  }

  [Test]
  public async Task IsSafe_WhenTheChangeIsTooSmall_ItShouldReturnFalse()
  {
    var report = new Report([8, 6, 4, 4, 1]);

    await Assert.That(report.IsSafe()).IsEqualTo(false);
  }

  [Test]
  public async Task IsSafeWithProblemDampener_WhenSafeByRemovingOneLevel_ItShouldReturnTrue()
  {
    var reportOne = new Report([1, 3, 2, 4, 5]);
    var reportTwo = new Report([8, 6, 4, 4, 1]);
    var reportThree = new Report([19, 21, 24, 27, 24]);

    await Assert.That(reportOne.IsSafeWithProblemDampener()).IsEqualTo(true);
    await Assert.That(reportTwo.IsSafeWithProblemDampener()).IsEqualTo(true);
    await Assert.That(reportThree.IsSafeWithProblemDampener()).IsEqualTo(true);
  }

  [Test]
  public async Task IsSafeWithProblemDampener_WhenSafeByRemovingNoLevels_ItShouldReturnTrue()
  {
    var reportOne = new Report([7, 6, 4, 2, 1]);
    var reportTwo = new Report([1, 3, 6, 7, 9]);

    await Assert.That(reportOne.IsSafeWithProblemDampener()).IsEqualTo(true);
    await Assert.That(reportTwo.IsSafeWithProblemDampener()).IsEqualTo(true);
  }

  [Test]
  public async Task IsSafeWithProblemDampener_WhenUnsafeRegardlessOfLevelsRemoved_ItShouldReturnFalse()
  {
    var reportOne = new Report([1, 2, 7, 8, 9]);
    var reportTwo = new Report([9, 7, 6, 2, 1]);

    await Assert.That(reportOne.IsSafeWithProblemDampener()).IsEqualTo(false);
    await Assert.That(reportTwo.IsSafeWithProblemDampener()).IsEqualTo(false);
  }
}