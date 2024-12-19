namespace ChronospatialComputer.Tests;

public class ComputerTests
{
  private readonly string _runProgramExampleInput = $"Register A: 729{Environment.NewLine}Register B: 0{Environment.NewLine}Register C: 0{Environment.NewLine}{Environment.NewLine}Program: 0,1,5,4,3,0";
  private readonly string _findCopyExampleInput = $"Register A: 2024{Environment.NewLine}Register B: 0{Environment.NewLine}Register C: 0{Environment.NewLine}{Environment.NewLine}Program: 0,3,5,4,3,0";
  
  [Test]
  public async Task RunProgram_WhenCalledWithExampleInput_ItShouldReturnExpectedValue()
  {
    var result = Computer.From(_runProgramExampleInput).RunProgram();

    await Assert.That(result).IsEqualTo("4,6,3,5,6,3,5,2,1,0");
  }
  
  [Test]
  public async Task FindCopy_WhenCalledWithExampleInput_ItShouldReturnExpectedValue()
  {
    var result = Computer.From(_findCopyExampleInput).FindCopy();

    await Assert.That(result).IsEqualTo(117440);
  }
}