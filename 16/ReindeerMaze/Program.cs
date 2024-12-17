using System.Diagnostics;

// if (args.Length is 0)
// {
//   Console.WriteLine("Please provide a path to the input file.");
//   return;
// }
//
// if (File.Exists(args[0]) is false)
// {
//   Console.WriteLine("The provided file does not exist.");
//   return;
// }

var isPart2 = args.Length is 2 && args[1] is "part2";
var input = await File.ReadAllLinesAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));

var stopwatch = new Stopwatch();
stopwatch.Start();

var result = PuzzleSolver.Solve(input);

stopwatch.Stop();
Console.WriteLine($"The lowest score a Reindeer could get is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

static class PuzzleSolver
{
  public static int Solve(string[] input)
  {
    List<(int Xd, int Yd)> directions = [
      (1, 0), 
      (0, 1), 
      (-1, 0),
      (0, -1),
    ];
    
    var rows = input.Length;
    var columns = input[0].Length;

    (int X, int Y) start = (-1, -1);
    (int X, int Y) end = (-1, -1);
    
    for (var row = 0; row < rows; row++)
    {
      for (var column = 0; column < columns; column++)
      {
        var currentValue = input[row][column];

        switch (currentValue)
        {
          case 'S':
            start = (column, row);
            break;
          case 'E':
            end = (column, row);
            break;
        }
      }
    }

    var queue = new PriorityQueue<(int X, int Y, int Direction, int Score), int>();
    var visited = new HashSet<(int X, int Y, int Direction)>();
    var initialScore = 0;
    var initialDirection = 0;
    
    queue.Enqueue((start.X, start.Y, initialDirection, initialScore), initialScore);
    
    while (queue.Count is not 0)
    {
      var current = queue.Dequeue();

      var possibleVisited = (current.X, current.Y, current.Direction);
      
      if (visited.Contains((possibleVisited)))
      {
        continue;
      }

      visited.Add(possibleVisited);

      if (current.X == end.X && current.Y == end.Y)
      {
        return current.Score;
      }

      var currentDirection = directions[current.Direction];
      var nextX = current.X + currentDirection.Xd;
      var nextY = current.Y + currentDirection.Yd;

      if (nextX >= 0 && nextX < columns && nextY >= 0 && nextY < rows && input[nextY][nextX] != '#')
      {
        var newScoreAfterMovingForward = current.Score + 1;
        queue.Enqueue((nextX, nextY, current.Direction, newScoreAfterMovingForward), newScoreAfterMovingForward);
      }
 
      var newScoreAfterTurning = current.Score + 1000;
      
      var directionAfterClockWiseTurn = (current.Direction + 1) % directions.Count;
      queue.Enqueue((current.X, current.Y, directionAfterClockWiseTurn, newScoreAfterTurning), newScoreAfterTurning);

      var directionAfterCounterClockWiseTurn = (current.Direction + 3) % directions.Count;
      queue.Enqueue((current.X, current.Y, directionAfterCounterClockWiseTurn, newScoreAfterTurning), newScoreAfterTurning);
    }
    
    return 0;
  }
}