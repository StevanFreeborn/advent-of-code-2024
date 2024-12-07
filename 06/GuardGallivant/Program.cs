using System.Diagnostics;

using GuardGallivant;

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
var input = await File.ReadAllLinesAsync(args[0]);

var stopwatch = new Stopwatch();
stopwatch.Start();

var map = new Map(input);
var result = isPart2 
  ? map.IdentifyNumberOfPlacementsForNewObstruction()
  : map.PredictDistinctPositionsCount();

var msg = isPart2
  ? $"There are {result} positions for the new obstruction"
  : $"The guard will visit {result} positions";

stopwatch.Stop();
Console.WriteLine($"{msg}. ({stopwatch.ElapsedMilliseconds}ms)");