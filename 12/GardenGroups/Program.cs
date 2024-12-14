using System.Diagnostics;

using GardenGroups;

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

var garden = Garden.From(input);
var result = isPart2
  ? garden.CalculateFencePriceBasedOnSides()
  : garden.CalculateFencePriceBasedOnPerimeter();

stopwatch.Stop();
Console.WriteLine($"The total cost to fence the garden is {result}. ({stopwatch.ElapsedMilliseconds}ms)");