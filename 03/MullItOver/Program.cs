using System.Diagnostics;

using MullItOver;

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

var parser = new PuzzleParser();

var instructions = isPart2 
  ? parser.ParseWithConditionals(input)
  : parser.Parse(input);

var results = instructions.Execute();

stopwatch.Stop();
Console.WriteLine($"The sum of instructions is {results}. ({stopwatch.ElapsedMilliseconds}ms)");