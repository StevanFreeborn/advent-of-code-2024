namespace PlutonianPebbles.Tests;

public class ObserverTests
{
  private static readonly string[] ExampleInput = ["125", "17"];

  private async Task<string> GetPuzzleInput()
  {
    return await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task Blink_WhenCalledWithPuzzleInputAnd25Times_ItShouldReturnCorrectNumberOfStones()
  {
    var input = await GetPuzzleInput();
    var stones = input
      .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
      .Select(Stone.From)
      .ToList();
    
    var observer = Observer.Of(stones);

    var result = observer.Blink(25);

    await Assert.That(result).IsEqualTo(191690);
  }
  
  [Test]
  public async Task Blink_WhenCalledWithPuzzleInputAnd75Times_ItShouldReturnCorrectNumberOfStones()
  {
    var input = await GetPuzzleInput();
    var stones = input
      .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
      .Select(Stone.From)
      .ToList();
    
    var observer = Observer.Of(stones);

    var result = observer.Blink(75);

    await Assert.That(result).IsEqualTo(228651922369703);
  }
  
  [Test]
  public async Task Blink_WhenCalledWithExampleInputAnd25Times_ItShouldReturnCorrectNumberOfStones()
  {
    var stones = ExampleInput.Select(Stone.From).ToList();
    var observer = Observer.Of(stones);

    var result = observer.Blink(25);

    await Assert.That(result).IsEqualTo(55312);
  }
  
  [Test]
  [MethodDataSource(nameof(BlinkTestCases))]
  public async Task Blink_WhenCalled_ItShouldReturnExpectedValue(BlinkTestCase testCase)
  {
    var stones = testCase.Stones.Select(Stone.From).ToList();
    var observer = Observer.Of(stones);

    var result = observer.Blink(testCase.NumberOfBlinks);
    
    await Assert.That(result).IsEqualTo(testCase.ExpectedNumberOfStones);
  }

  public static IEnumerable<Func<BlinkTestCase>> BlinkTestCases()
  {
    yield return () => new(ExampleInput, 0, 2);
    yield return () => new(ExampleInput, 1, 3);
    yield return () => new(ExampleInput, 2, 4);
    yield return () => new(ExampleInput, 3, 5);
    yield return () => new(ExampleInput, 4, 9);
    yield return () => new(ExampleInput, 5, 13);
    yield return () => new(ExampleInput, 6, 22);
  }

  public record BlinkTestCase(string[] Stones, int NumberOfBlinks, long ExpectedNumberOfStones);
}