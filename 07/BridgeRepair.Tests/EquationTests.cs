namespace BridgeRepair.Tests;

public class EquationTests
{
  [Test]
  [MethodDataSource(nameof(GeneratePossibleOperatorConfigurationsTestCases))]
  public async Task GeneratePossibleOperatorConfigurations_WhenCalled_ItShouldGenerateExpectedCombinations(GeneratePossibleOperatorConfigurationsTestCase testCase)
  {
    var result = new Equation(testCase.TargetValue, testCase.Numbers).GeneratePossibleOperatorConfigurations().ToList();

    await Assert.That(result).IsEquivalentTo(testCase.ExpectedCombinations);
  }
  
  [Test]
  [MethodDataSource(nameof(EvaluateTestCases))]
  public async Task Evaluate_WhenCalled_ItShouldReturnExpectedValue(EvaluateTestCase testCase)
  {
    var result = new Equation(testCase.TargetValue, testCase.Numbers).Evaluate(testCase.Combinations);

    await Assert.That(result).IsEquivalentTo(testCase.ExpectedValue);
  }
  
  [Test]
  [MethodDataSource(nameof(IsPossibleTestCases))]
  public async Task IsPossible_WhenCalled_ItShouldReturnExpectedValue(IsPossibleTestCase testCase)
  {
    var result = new Equation(testCase.TargetValue, testCase.Numbers).IsPossible();

    await Assert.That(result).IsEquivalentTo(testCase.ExpectedValue);
  }
  
  [Test]
  public async Task IsPossible_WhenCalledWithPart2_ItShouldReturnExpectedValue()
  {
    // TODO: Make data driven test
    var resultOne = new Equation(156, [15, 6]).IsPossible(isPart2: true);
    var resultTwo = new Equation(7290, [6, 8, 6, 15]).IsPossible(isPart2: true);
    var resultThree = new Equation(192, [17, 8, 14]).IsPossible(isPart2: true);
    
    await Assert.That(resultOne).IsTrue();
    await Assert.That(resultTwo).IsTrue();
    await Assert.That(resultThree).IsTrue();
  }
  
  public static IEnumerable<Func<IsPossibleTestCase>> IsPossibleTestCases()
  {
    yield return () => new(190, [10, 19], true);
    yield return () => new(3267, [81, 40, 27], true);
    yield return () => new(83, [17, 5], false);
    yield return () => new(156, [15, 6], false);
    yield return () => new(7290, [6, 8, 6, 15], false);
    yield return () => new(161011, [16, 10, 13], false);
    yield return () => new(192, [17, 8, 14], false);
    yield return () => new(21037, [9, 7, 18, 13], false);
    yield return () => new(292, [11, 6, 16, 20], true);
  }

  public record IsPossibleTestCase(long TargetValue, List<long> Numbers, bool ExpectedValue);

  public static IEnumerable<Func<EvaluateTestCase>> EvaluateTestCases()
  {
    yield return () => new(190, [10, 19], ['+'], 29);
    yield return () => new(190, [10, 19], ['*'], 190);
    yield return () => new(3267, [81, 40, 27], ['*', '+'], 3267);
    yield return () => new(292, [11, 6, 16, 20], ['+', '*', '+'], 292);
  }
  
  public record EvaluateTestCase(long TargetValue, List<long> Numbers, char[] Combinations, long ExpectedValue);

  public static IEnumerable<Func<GeneratePossibleOperatorConfigurationsTestCase>> GeneratePossibleOperatorConfigurationsTestCases()
  {
    yield return () => new(190, [10, 19], [['+'], ['*']]);
    yield return () => new(3267, [81, 40, 27], [['+', '+'], ['*', '+'], ['+', '*'], ['*', '*']]);
    yield return () => new(292, [11, 6, 16, 20], [
      ['+', '+', '+'],
      ['*', '+', '+'],
      ['+', '*', '+'],
      ['*', '*', '+'],
      ['+', '+', '*'],
      ['*', '+', '*'],
      ['+', '*', '*'],
      ['*', '*', '*'],
    ]);
  }
    
  public record GeneratePossibleOperatorConfigurationsTestCase(long TargetValue, List<long> Numbers, List<char[]> ExpectedCombinations);
}