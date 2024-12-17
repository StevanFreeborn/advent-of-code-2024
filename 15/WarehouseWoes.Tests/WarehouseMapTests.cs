namespace WarehouseWoes.Tests;

public class WarehouseMapTests
{
  private static readonly string[] LargeExampleMap = [
    "##########",
    "#..O..O.O#",
    "#......O.#",
    "#.OO..O.O#",
    "#..O@..O.#",
    "#O#..O...#",
    "#O..O..O.#",
    "#.OO.O.OO#",
    "#....O...#",
    "##########",
  ];

  private static readonly string[] SmallExampleMap = [
    "########",
    "#..O.O.#",
    "##@.O..#",
    "#...O..#",
    "#.#.O..#",
    "#...O..#",
    "#......#",
    "########",
  ];

  private static readonly string[] LargeExampleDirections = [
    "<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^",
    "vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v",
    "><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<",
    "<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^",
    "^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><",
    "^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^",
    ">^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^",
    "<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>",
    "^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>",
    "v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^",
  ];

  private static readonly string[] SmallExampleDirections = [
    "<^^>>>vv<v>>v<<",
  ];

  [Test]
  [MethodDataSource(nameof(ScaleTestCases))]
  public async Task Scale_WhenCalled_ItShouldReturnExpectedMap(ScaleTestCase testCase)
  {
    var expected = WideWarehouseMap.From(testCase.ExpectedMap);

    var result = WarehouseMap.From(testCase.MapInput).Scale();

    await Assert.That(result.Positions).IsEquivalentTo(expected.Positions);
  }

  public static IEnumerable<Func<ScaleTestCase>> ScaleTestCases()
  {
    yield return () => new(
      [
        "##########",
        "#..O..O.O#",
        "#......O.#",
        "#.OO..O.O#",
        "#..O@..O.#",
        "#O#..O...#",
        "#O..O..O.#",
        "#.OO.O.OO#",
        "#....O...#",
        "##########",
      ],
      [
        "####################",
        "##....[]....[]..[]##",
        "##............[]..##",
        "##..[][]....[]..[]##",
        "##....[]@.....[]..##",
        "##[]##....[]......##",
        "##[]....[]....[]..##",
        "##..[][]..[]..[][]##",
        "##........[]......##",
        "####################",
      ]
    );
    
    yield return () => new(
      [
        "#######",
        "#...#.#",
        "#.....#",
        "#..OO@#",
        "#..O..#",
        "#.....#",
        "#######",
      ],
      [
        "##############",
        "##......##..##",
        "##..........##",
        "##....[][]@.##",
        "##....[]....##",
        "##..........##",
        "##############",
      ]
    );
  }
  
  public record ScaleTestCase(string[] MapInput, string[] ExpectedMap);

  [Test]
  [MethodDataSource(nameof(MoveTestCases))]
  public async Task Move_WhenCalled_ItShouldMoveValuesToExpectedPositions(MoveTestCase testCase)
  {
    var expected = WarehouseMap.From(testCase.ExpectedMap);
    var directions = testCase.Directions.Select(Direction.From).ToList();

    var result = WarehouseMap.From(testCase.MapInput).MoveRobot(directions);

    await Assert.That(result.Positions).IsEquivalentTo(expected.Positions);
  }

  public static IEnumerable<Func<MoveTestCase>> MoveTestCases()
  {
    yield return () => new(
     [
      "########",
      "#..O.O.#",
      "##@.O..#",
      "#...O..#",
      "#.#.O..#",
      "#...O..#",
      "#......#",
      "########",
     ],
     "<",
     [
      "########",
      "#..O.O.#",
      "##@.O..#",
      "#...O..#",
      "#.#.O..#",
      "#...O..#",
      "#......#",
      "########",
     ]
    );
    
    yield return () => new(
      [
        "########",
        "#..O.O.#",
        "##@.O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ],
      "^",
      [
        "########",
        "#.@O.O.#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#.@O.O.#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ],
      "^",
      [
        "########",
        "#.@O.O.#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#.@O.O.#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ],
      ">",
      [
        "########",
        "#..@OO.#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#..@OO.#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ],
      ">",
      [
        "########",
        "#...@OO#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#...@OO#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ],
      ">",
      [
        "########",
        "#...@OO#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#...@OO#",
        "##..O..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#......#",
        "########",
      ],
      "v",
      [
        "########",
        "#....OO#",
        "##..@..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##..@..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      "v",
      [
        "########",
        "#....OO#",
        "##..@..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##..@..#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      "<",
      [
        "########",
        "#....OO#",
        "##.@...#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##.@...#",
        "#...O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      "v",
      [
        "########",
        "#....OO#",
        "##.....#",
        "#..@O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##.....#",
        "#..@O..#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      ">",
      [
        "########",
        "#....OO#",
        "##.....#",
        "#...@O.#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##.....#",
        "#...@O.#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      ">",
      [
        "########",
        "#....OO#",
        "##.....#",
        "#....@O#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##.....#",
        "#....@O#",
        "#.#.O..#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      "v",
      [
        "########",
        "#....OO#",
        "##.....#",
        "#.....O#",
        "#.#.O@.#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##.....#",
        "#.....O#",
        "#.#.O@.#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      "<",
      [
        "########",
        "#....OO#",
        "##.....#",
        "#.....O#",
        "#.#O@..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
    
    yield return () => new(
      [
        "########",
        "#....OO#",
        "##.....#",
        "#.....O#",
        "#.#O@..#",
        "#...O..#",
        "#...O..#",
        "########",
      ],
      "<",
      [
        "########",
        "#....OO#",
        "##.....#",
        "#.....O#",
        "#.#O@..#",
        "#...O..#",
        "#...O..#",
        "########",
      ]
    );
  }

  public record MoveTestCase(string[] MapInput, string Directions, string[] ExpectedMap);

  [Test]
  [MethodDataSource(nameof(CalculateTotalBoxGpsCoordinatesTestCases))]
  public async Task CalculateTotalBoxGpsCoordinates_WhenCalledWithExampleInput_ItShouldReturnExpectedValue(CalculateTotalBoxGpsCoordinatesTestCase testCase)
  {
    var map = WarehouseMap.From(testCase.MapInput);
    var directions = string.Join(string.Empty, testCase.DirectionsInput).Select(Direction.From).ToList();

    var mapAfterRobotMoved = map.MoveRobot(directions);
    var result = mapAfterRobotMoved.CalculateTotalBoxGpsCoordinates();

    await Assert.That(result).IsEqualTo(testCase.ExpectedValue);
  }
  
  public static IEnumerable<Func<CalculateTotalBoxGpsCoordinatesTestCase>> CalculateTotalBoxGpsCoordinatesTestCases()
  {
    yield return () => new(LargeExampleMap, LargeExampleDirections, 10092);
    yield return () => new(SmallExampleMap, SmallExampleDirections, 2028);
  }

  public record CalculateTotalBoxGpsCoordinatesTestCase(string[] MapInput, string[] DirectionsInput, int ExpectedValue);
}