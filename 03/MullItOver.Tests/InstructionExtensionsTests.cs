namespace MullItOver.Tests;

public class InstructionExtensionsTests
{
  [Test]
  public async Task Execute_WhenCalledWithNoConditionals_ItShouldReturnCorrectSum()
  {
    var instructions = new List<Instruction>()
    {
      new MulInstruction(2, 4), 
      new MulInstruction(5, 5), 
      new MulInstruction(11, 8), 
      new MulInstruction(8, 5),
    };

    var result = instructions.Execute();

    await Assert.That(result).IsEqualTo(161);
  }

  [Test]
  public async Task Execute_WhenCalledWithConditionals_ItShouldReturnCorrectSum()
  {
    var instructions = new List<Instruction>()
    {
      new MulInstruction(2, 4),
      new DontInstruction(),
      new MulInstruction(5, 5),
      new MulInstruction(11, 8),
      new DoInstruction(),
      new MulInstruction(8, 5),
    };

    var result = instructions.Execute();

    await Assert.That(result).IsEqualTo(48);
  }
}