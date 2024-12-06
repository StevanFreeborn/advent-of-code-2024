namespace PrintQueue.Tests;

public class UpdateValidatorTests
{
  private readonly List<OrderRule> _exampleRules = [
    new(47, 53),
    new(97, 13),
    new(97, 61),
    new(97, 47),
    new(75, 29),
    new(61, 13),
    new(75, 53),
    new(29, 13),
    new(97, 29),
    new(53, 29),
    new(61, 53),
    new(97, 53),
    new(61, 29),
    new(47, 13),
    new(75, 47),
    new(97, 75),
    new(47, 61),
    new(75, 61),
    new(47, 29),
    new(75, 13),
    new(53, 13),
  ];

  [Test]
  public async Task Graph_WhenGivenExampleRules_ItShouldBuildExpectedGraph()
  {
    var expectedGraph = new Dictionary<int, HashSet<int>>()
    {
      { 47, [53, 13, 61, 29] },
      { 53, [29, 13] },
      { 97, [13, 61, 47, 29, 53, 75] },
      { 13, [] },
      { 61, [13, 53, 29] },
      { 75, [29, 53, 47, 61, 13] },
      { 29, [13] },
    };
    
    var validator = new UpdateValidator(_exampleRules);
    
    await Assert.That(validator.Graph.Keys).IsEquivalentTo(expectedGraph.Keys);
    await Assert.That(validator.Graph.Values).IsEquivalentTo(expectedGraph.Values);
  }

  [Test]
  public async Task Validate_WhenGivenValidUpdate_ItShouldReturnTrue()
  {
    var updateOne = new Update([75, 47, 61, 53, 29]);
    var updateTwo = new Update([97,61,53,29,13]);
    var updateThree = new Update([75,29,13]);

    var validator = new UpdateValidator(_exampleRules);
    
    var resultOne = validator.Validate(updateOne);
    var resultTwo = validator.Validate(updateTwo);
    var resultThree = validator.Validate(updateThree);
    
    await Assert.That(resultOne).IsTrue();
    await Assert.That(resultTwo).IsTrue();
    await Assert.That(resultThree).IsTrue();
  }

  [Test]
  public async Task Validate_WhenGivenInvalidUpdate_ItShouldReturnFalse()
  {
    var updateOne = new Update([75,97,47,61,53]);
    var updateTwo = new Update([61,13,29]);
    var updateThree = new Update([97,13,75,29,47]);

    var validator = new UpdateValidator(_exampleRules);
    
    var resultOne = validator.Validate(updateOne);
    var resultTwo = validator.Validate(updateTwo);
    var resultThree = validator.Validate(updateThree);
    
    await Assert.That(resultOne).IsFalse();
    await Assert.That(resultTwo).IsFalse();
    await Assert.That(resultThree).IsFalse();
  }
}