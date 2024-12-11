namespace PlutonianPebbles.Tests;

public class ObserverTests
{
  private static readonly string[] ExampleInput = ["125", "17"];
  
  [Test]
  [MethodDataSource(nameof(BlinkTestCases))]
  public async Task Blink_WhenCalled_ItShouldReturnExpectedValue(BlinkTestCase testCase)
  {
    var stones = testCase.Stones.Select(Stone.From).ToList();
    var observer = Observer.Of(stones);

    var result = observer.Blink(testCase.NumberOfBlinks);

    await Assert.That(result).IsEqualTo(testCase.ExpectedLineOfStones);
  }

  public static IEnumerable<Func<BlinkTestCase>> BlinkTestCases()
  {
    yield return () => new(ExampleInput, 0, "125 17");
  }

  public record BlinkTestCase(string[] Stones, int NumberOfBlinks, string ExpectedLineOfStones);
}