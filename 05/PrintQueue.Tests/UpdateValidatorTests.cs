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
  [MethodDataSource(nameof(TestCases))]
  public async Task Validate_WhenUpdate_ItShouldReturnExpectedResult(TestCase testCase)
  {
    var update = new Update(testCase.Pages);

    var validator = new UpdateValidator(_exampleRules);
    
    var result = validator.Validate(update);
    
    await Assert.That(result).IsEquivalentTo(testCase.ExpectedValidateResult);
  }

  [Test]
  [MethodDataSource(nameof(TestCases))]
  public async Task Sort_WhenGivenUpdate_ItShouldReturnUpdateSorted(TestCase testCase)
  {
    var update = new Update(testCase.Pages);

    var validator = new UpdateValidator(_exampleRules);
    
    var result = validator.Sort(update);

    await Assert.That(result).IsEquivalentTo(new Update(testCase.SortedPages));
  }
  
  public static IEnumerable<Func<TestCase>> TestCases()
  {
    yield return () =>
    {
      List<int> pages = [ 75, 47, 61, 53, 29 ];
      return new TestCase(pages, true, pages);
    };
    yield return () =>
    {
      List<int> pages = [ 97,61,53,29,13 ];
      return new TestCase(pages, true, pages);
    };
    yield return () =>
    {
      List<int> pages = [ 75,29,13 ];
      return new TestCase(pages, true, pages);
    };
    yield return () => new TestCase([75,97,47,61,53], false, [97,75,47,61,53]);
    yield return () => new TestCase([61,13,29], false, [61,29,13]);
    yield return () => new TestCase([97,13,75,29,47], false, [97,75,47,29,13]);
  }
    
  public record TestCase(List<int> Pages, bool ExpectedValidateResult, List<int> SortedPages);
}