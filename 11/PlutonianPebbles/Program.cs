using System.Diagnostics;

using PlutonianPebbles;

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

var stones = input
  .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
  .Select(Stone.From)
  .ToList();

var observer = Observer.Of(stones);

var times = isPart2 ? 75 : 25;

var result = observer.Blink(times);

stopwatch.Stop();
Console.WriteLine($"The number of stones is {result}. ({stopwatch.ElapsedMilliseconds}ms)");