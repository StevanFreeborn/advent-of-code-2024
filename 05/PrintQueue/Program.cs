using System.Diagnostics;

using PrintQueue;

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

var parseResult = parser.Parse(input);

var validator = new UpdateValidator(parseResult.Rules);

var result = isPart2 
  ? parseResult.Updates
      .Where(u => validator.Validate(u) is false)
      .Select(u => validator.Sort(u))
      .Select(u => u.GetMiddlePage())
      .Sum()
  : parseResult.Updates
      .Where(u => validator.Validate(u))
      .Select(u => u.GetMiddlePage())
      .Sum();

stopwatch.Stop();
Console.WriteLine($"The sum of the page numbers is {result}. ({stopwatch.ElapsedMilliseconds}ms)");