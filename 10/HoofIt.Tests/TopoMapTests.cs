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
  
  [Test]
  public async Task From_WhenCalled_ItShouldReturnNewInstanceOfTopoMap()
  {
    var result = TopoMap.From(_exampleInput);

    await Assert.That(result).IsTypeOf<TopoMap>();
  }

  [Test]
  public async Task GetTrailHeadScores_WhenCalledWithExampleInput_ItShouldReturnExpectedValues()
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

    var result = TopoMap.From(_exampleInput).GetTrailHeadScores();
    
    await Assert.That(result).IsEqualTo(36);
  }
}