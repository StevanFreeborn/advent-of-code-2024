namespace PlutonianPebbles.Tests;

public class StoneTests
{
  [Test]
  public async Task ToString_WhenCalled_ItShouldReturnEngravedValue()
  {
    var engravedValue = "0";
    
    var result = Stone.From(engravedValue).ToString();

    await Assert.That(result).IsEqualTo(engravedValue);
  }
  
  [Test]
  [MethodDataSource(nameof(TransformTestCases))]
  public async Task Transform_WhenCalled_ItShouldReturnStonedWithExpectedEngraving(TransformTestCase testCase)
  {
    var expected = testCase.TransformedValues.Select(Stone.From).ToList();
    
    var result = Stone.From(testCase.EngravedValue).Transform();

    await Assert.That(result).IsEquivalentTo(expected);
  }
  
  public static IEnumerable<Func<TransformTestCase>> TransformTestCases()
  {
    yield return () => new("0", ["1"]);
    yield return () => new("1", ["2024"]);
    yield return () => new("10", ["1", "0"]);
    yield return () => new("99", ["9", "9"]);
    yield return () => new("999", ["2021976"]);
  }
  
  public record TransformTestCase(string EngravedValue, List<string> TransformedValues);
}