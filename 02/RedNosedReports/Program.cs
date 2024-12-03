using System.Diagnostics;

using RedNosedReports;

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

var reports = new PuzzleParser().Parse(input);

var numberOfSafeReports = isPart2
  ? reports.Count(r => r.IsSafeWithProblemDampener())
  : reports.Count(r => r.IsSafe());

stopwatch.Stop();
Console.WriteLine($"The total number of safe reports is {numberOfSafeReports}. ({stopwatch.ElapsedMilliseconds}ms)");