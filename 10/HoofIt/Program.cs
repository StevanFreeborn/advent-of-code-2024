using System.Diagnostics;

using HoofIt;

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

var map = TopoMap.From(input);

var nums = isPart2
  ? map.GetTrailheadRatings()
  : map.GetTrailheadScores();

var result = nums.Sum();

var nameOfStat = isPart2 ? "ratings" : "scores";

stopwatch.Stop();
Console.WriteLine($"The sum of the {nameOfStat} of all trailheads is {result}. ({stopwatch.ElapsedMilliseconds}ms)");