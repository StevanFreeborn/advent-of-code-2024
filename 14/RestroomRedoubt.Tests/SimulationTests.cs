namespace RestroomRedoubt.Tests;

public class SimulationTests
{
  private readonly string[] _exampleInput = [
    "p=0,4 v=3,-3",
    "p=6,3 v=-1,-3",
    "p=10,3 v=-1,2",
    "p=2,0 v=2,-1",
    "p=0,0 v=1,3",
    "p=3,0 v=-2,-2",
    "p=7,6 v=-1,-3",
    "p=3,0 v=-1,-2",
    "p=9,3 v=2,3",
    "p=7,3 v=-1,2",
    "p=2,4 v=2,-3",
    "p=9,5 v=-3,-3",
  ];

  private async Task<string[]> GetPuzzleInput()
  {
    return await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }

  [Test]
  public async Task Run_WhenCalledWithPuzzleInput_ItShouldReturnPartOneSolution()
  {
    var input = await GetPuzzleInput();

    var simulation = Simulation.From(101, 103, input);
    simulation.Run(100);

    await Assert.That(simulation.SafetyFactor).IsEqualTo(224554908);
  }
  
  [Test]
  public async Task Run_WhenCalledWithPuzzleInput_ItShouldReturnPartTwoSolution()
  {
    var input = await GetPuzzleInput();

    var width = 101;
    var height = 103;
    var times = width * height;
    
    var result = Simulation.From(width, height, input).Run(times).MinBy(kvp => kvp.Value).Key;

    await Assert.That(result).IsEqualTo(0);
  }
  
  [Test]
  public async Task Run_WhenCalledWithExampleInput_ItShouldReturnExpectedValue()
  {
    var simulation = Simulation.From(11, 7, _exampleInput);

    simulation.Run(100);

    await Assert.That(simulation.SafetyFactor).IsEqualTo(12);
  }
}