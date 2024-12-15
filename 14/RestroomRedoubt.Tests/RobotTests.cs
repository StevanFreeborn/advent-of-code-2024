namespace RestroomRedoubt.Tests;

public class RobotTests
{
  [Test]
  [MethodDataSource(nameof(FromTestCases))]
  public async Task From_WhenCalledWithInput_ItShouldReturnExpectedRobot(FromTestCase testCase)
  {
    var expected = Robot.From(
      testCase.ExpectedPosition.X,
      testCase.ExpectedPosition.Y,
      testCase.ExpectedVelocity.X,
      testCase.ExpectedVelocity.Y
    );

    var result = Robot.From(testCase.Input);

    await Assert.That(result).IsEqualTo(expected);
  }

  [Test]
  [MethodDataSource(nameof(MoveTestCases))]
  public async Task Move_WhenCalled_ItShouldMoveRobotToExpectedPosition(MoveTestCase testCase)
  {
    var robot = Robot.From(
      testCase.CurrentPosition.X,
      testCase.CurrentPosition.Y,
      testCase.Velocity.X,
      testCase.Velocity.Y
    );

    var result = robot.Move(testCase.Area.Width, testCase.Area.Height);

    await Assert.That(result).IsEqualTo(testCase.ExpectedNextPosition);
  }

  public static IEnumerable<Func<MoveTestCase>> MoveTestCases()
  {
    var testVelocity = (2, -3);
    var testArea = (11, 7);
    
    yield return () => new((2, 4), testVelocity, testArea, (4, 1));
    yield return () => new((4, 1), testVelocity, testArea, (6, 5));
    yield return () => new((6, 5), testVelocity, testArea, (8, 2));
    yield return () => new((8, 2), testVelocity, testArea, (10, 6));
    yield return () => new((10, 6), testVelocity, testArea, (1, 3));
  }

  public record MoveTestCase(
    (int X, int Y) CurrentPosition, 
    (int X, int Y) Velocity,
    (int Width, int Height) Area,
    (int X, int Y) ExpectedNextPosition
  );

  public static IEnumerable<Func<FromTestCase>> FromTestCases()
  {
    yield return () => new("p=0,4 v=3,-3", (0, 4), (3, -3));
    yield return () => new("p=6,3 v=-1,-3", (6, 3), (-1, -3));
    yield return () => new("p=10,3 v=-1,2", (10, 3), (-1, 2));
    yield return () => new("p=2,0 v=2,-1", (2, 0), (2, -1));
    yield return () => new("p=0,0 v=1,3", (0, 0), (1, 3));
    yield return () => new("p=3,0 v=-2,-2", (3, 0), (-2, -2));
    yield return () => new("p=7,6 v=-1,-3", (7, 6), (-1, -3));
    yield return () => new("p=3,0 v=-1,-2", (3, 0), (-1, -2));
    yield return () => new("p=9,3 v=2,3", (9, 3), (2, 3));
    yield return () => new("p=7,3 v=-1,2", (7, 3), (-1, 2));
    yield return () => new("p=2,4 v=2,-3", (2, 4), (2, -3));
    yield return () => new("p=9,5 v=-3,-3", (9, 5), (-3, -3));
  }
  
  public record FromTestCase(string Input, (int X, int Y) ExpectedPosition, (int X, int Y) ExpectedVelocity);
}