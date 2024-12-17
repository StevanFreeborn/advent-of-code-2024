using System.Diagnostics;

using WarehouseWoes;

if (args.Length is 0)
{
  Console.WriteLine("Please provide a path to the input file.");
  return;
}

if (File.Exists(args[0]) is false)
{
  Console.WriteLine("The provided file does not exist.");
  return;
}

var isPart2 = args.Length is 2 && args[1] is "part2";
var input = await File.ReadAllTextAsync(args[0]);

var stopwatch = new Stopwatch();
stopwatch.Start();

var (parsedMap, directions) = PuzzleParser.Parse(input);

var map = isPart2 
  ? parsedMap.Scale() 
  : parsedMap;

var result = map.MoveRobot(directions).CalculateTotalBoxGpsCoordinates();

stopwatch.Stop();
Console.WriteLine($"The sum of all boxes' GPS coordinates is {result}. ({stopwatch.ElapsedMilliseconds}ms)");