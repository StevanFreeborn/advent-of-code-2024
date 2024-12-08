using System.Diagnostics;

using BridgeRepair;

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

var result = new PuzzleParser().Parse(input)
  .Where(e => e.IsPossible(isPart2))
  .Sum(e => e.TestValue);

stopwatch.Stop();
Console.WriteLine($"The sum of the test value for possible equations is {result}. ({stopwatch.ElapsedMilliseconds}ms)");