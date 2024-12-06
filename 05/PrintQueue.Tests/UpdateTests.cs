namespace PrintQueue.Tests;

public class UpdateTests
{
  [Test]
  public async Task GetMiddlePage_WhenCalled_ItShouldReturnExpectedPage()
  {
    var updates = new List<Update>()
    {
      new([75,47,61,53,29]),
      new([97,61,53,29,13]),
      new([75,29,13]),
    };

    var result = updates.Select(u => u.GetMiddlePage()).Sum();

    await Assert.That(result).IsEqualTo(143);
  }
}