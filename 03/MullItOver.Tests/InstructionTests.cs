namespace MullItOver.Tests;

public class InstructionTests
{
  [Test]
  public async Task Execute_WhenCalled_ItShouldReturnProductOfMultiplicands()
  {
    var result = new MulInstruction(2, 2).Execute();

    await Assert.That(result).IsEquivalentTo(4);
  }
}