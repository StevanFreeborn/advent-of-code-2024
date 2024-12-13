namespace GardenGroups.Tests;

public class GardenTests
{
  [Test]
  [MethodDataSource(nameof(CalculateFencePriceBasedOnPerimeterTestCases))]
  public async Task CalculateFencePriceBasedOnPerimeter_WhenCalled_ItShouldReturnExpectedValue(CalculateFencePriceTestCase testCase)
  {
    var result = Garden.From(testCase.Input).CalculateFencePriceBasedOnPerimeter();

    await Assert.That(result).IsEqualTo(testCase.ExpectedPrice);
  }
  
  [Test]
  [MethodDataSource(nameof(CalculateFencePriceBasedOnSidesTestCases))]
  public async Task CalculateFencePriceBasedOnSides_WhenCalled_ItShouldReturnExpectedValue(CalculateFencePriceTestCase testCase)
  {
    var result = Garden.From(testCase.Input).CalculateFencePriceBasedOnSides();

    await Assert.That(result).IsEqualTo(testCase.ExpectedPrice);
  }

  public static IEnumerable<Func<CalculateFencePriceTestCase>> CalculateFencePriceBasedOnSidesTestCases()
  {
    yield return () => new(["AAAA", "BBCD", "BBCC", "EEEC"], 80);
    
    yield return () => new(
      [
        "EEEEE",
        "EXXXX",
        "EEEEE",
        "EXXXX",
        "EEEEE",
      ],
      236
    );
    
    yield return () => new(
      [
        "AAAAAA",
        "AAABBA",
        "AAABBA",
        "ABBAAA",
        "ABBAAA",
        "AAAAAA",
      ],
      368
    );
    
    yield return () => new(
      [
        "RRRRIICCFF",
        "RRRRIICCCF",
        "VVRRRCCFFF",
        "VVRCCCJFFF",
        "VVVVCJJCFE",
        "VVIVCCJJEE",
        "VVIIICJJEE",
        "MIIIIIJJEE",
        "MIIISIJEEE",
        "MMMISSJEEE",
      ],
      1206
    );
  }

  public static IEnumerable<Func<CalculateFencePriceTestCase>> CalculateFencePriceBasedOnPerimeterTestCases()
  {
    yield return () => new(["AAAA", "BBCD", "BBCC", "EEEC"], 140);
    
    yield return () => new(
      [
        "OOOOO",
        "OXOXO",
        "OOOOO",
        "OXOXO",
        "OOOOO",
      ], 
      772
    );
    
    yield return () => new(
      [
       "RRRRIICCFF",
       "RRRRIICCCF",
       "VVRRRCCFFF",
       "VVRCCCJFFF",
       "VVVVCJJCFE",
       "VVIVCCJJEE",
       "VVIIICJJEE",
       "MIIIIIJJEE",
       "MIIISIJEEE",
       "MMMISSJEEE",
      ],
      1930
    );
  }
  
  public record CalculateFencePriceTestCase(string[] Input, int ExpectedPrice);
}