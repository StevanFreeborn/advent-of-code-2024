namespace ClawContraption.Tests;

public class MachineTests
{
  private async Task<List<string[]>> GetPuzzleInput()
  {
    var input = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
    
    return input
      .Split($"{Environment.NewLine}{Environment.NewLine}")
      .Select(s => s.Split(Environment.NewLine))
      .ToList();
  }
  
  [Test]
  public async Task SolutionPartOne_WhenGivenPuzzleInput_ItShouldReturnExpectedResult()
  {
    var input = await GetPuzzleInput();
    var machines = input.Select(s => Machine.From(s, 100));
    var result = machines.Sum(m => m.TokenCost);

    await Assert.That(result).IsEqualTo(28262);
  }
  
  [Test]
  public async Task SolutionPartTwo_WhenGivenPuzzleInput_ItShouldReturnExpectedResult()
  {
    var input = await GetPuzzleInput();
    var machines = input.Select(s => Machine.From(s, maxPresses: null, 10_000_000_000_000));
    var result = machines.Sum(m => m.TokenCost);

    await Assert.That(result).IsEqualTo(101406661266314);
  }
  
  [Test]
  [MethodDataSource(nameof(FromTestCases))]
  public async Task From_WhenCalledWithInput_ItShouldReturnExpectedMachine(FromTestCase testCase)
  {
    var expectedMachine = Machine.From(
      buttonA: new(testCase.ExpectedButtonAValues.X, testCase.ExpectedButtonAValues.Y),
      buttonB: new(testCase.ExpectedButtonBValues.X, testCase.ExpectedButtonBValues.Y),
      prizeLocation: new(
        testCase.ExpectedPrizeValues.X + testCase.PrizeLocationOffset,
        testCase.ExpectedPrizeValues.Y + testCase.PrizeLocationOffset
      ),
      maxPresses: testCase.MaxPresses
    );
    
    var machine = Machine.From(testCase.MachineInput, testCase.MaxPresses, testCase.PrizeLocationOffset);
    
    await Assert.That(machine).IsEquivalentTo(expectedMachine);
    await Assert.That(machine.TokenCost).IsEqualTo(testCase.ExpectedTokenCost);
  }

  public static IEnumerable<Func<FromTestCase>> FromTestCases()
  {
    yield return () => new(
      [
        "Button A: X+94, Y+34",
        "Button B: X+22, Y+67",
        "Prize: X=8400, Y=5400",
      ],
      0,
      100,
      (94, 34),
      (22, 67),
      (8400, 5400),
      280
    );

    yield return () => new(
      [
        "Button A: X+26, Y+66",
        "Button B: X+67, Y+21",
        "Prize: X=12748, Y=12176",
      ],
      0,
      100,
      (26, 66),
      (67, 21),
      (12748, 12176),
      0
    );
    
    yield return () => new(
      [
        "Button A: X+17, Y+86",
        "Button B: X+84, Y+37",
        "Prize: X=7870, Y=6450",
      ],
      0,
      100,
      (17, 86),
      (84, 37),
      (7870, 6450),
      200
    );
    
    yield return () => new(
      [
        "Button A: X+69, Y+23",
        "Button B: X+27, Y+71",
        "Prize: X=18641, Y=10279",
      ],
      0,
      100,
      (69, 23),
      (27, 71),
      (18641, 10279),
      0
    );
    
    // part 2 cases
    yield return () => new(
      [
        "Button A: X+94, Y+34",
        "Button B: X+22, Y+67",
        "Prize: X=8400, Y=5400",
      ],
      10_000_000_000_000,
      null,
      (94, 34),
      (22, 67),
      (8400, 5400),
      0
    );

    yield return () => new(
      [
        "Button A: X+26, Y+66",
        "Button B: X+67, Y+21",
        "Prize: X=12748, Y=12176",
      ],
      10_000_000_000_000,
      null,
      (26, 66),
      (67, 21),
      (12748, 12176),
      459236326669
    );
    
    yield return () => new(
      [
        "Button A: X+17, Y+86",
        "Button B: X+84, Y+37",
        "Prize: X=7870, Y=6450",
      ],
      10_000_000_000_000,
      null,
      (17, 86),
      (84, 37),
      (7870, 6450),
      0
    );
    
    yield return () => new(
      [
        "Button A: X+69, Y+23",
        "Button B: X+27, Y+71",
        "Prize: X=18641, Y=10279",
      ],
      10_000_000_000_000,
      null,
      (69, 23),
      (27, 71),
      (18641, 10279),
      416082282239
    );
  }

  public record FromTestCase(
    string[] MachineInput,
    long PrizeLocationOffset,
    int? MaxPresses,
    (int X, int Y) ExpectedButtonAValues,
    (int X, int Y) ExpectedButtonBValues,
    (int X, int Y) ExpectedPrizeValues,
    decimal ExpectedTokenCost
  );
}