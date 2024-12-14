using System.Diagnostics;

using ClawContraption;

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

var offset = isPart2 ? 10_000_000_000_000 : 0;
int? maxPresses = isPart2 ? null : 100;

var machines = input
  .Split($"{Environment.NewLine}{Environment.NewLine}")
  .Select(s => s.Split(Environment.NewLine))
  .Select(s => Machine.From(s, maxPresses, offset))
  .ToList();

var result = machines.Sum(m => m.TokenCost);

stopwatch.Stop();
Console.WriteLine($"The fewest tokens to when all possible games is {result}. ({stopwatch.ElapsedMilliseconds}ms)");