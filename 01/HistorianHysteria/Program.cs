using System.Diagnostics;

using HistorianHysteria;

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

var (left, right) = new PuzzleParser().Parse(input);

if (isPart2)
{
  var similarityScore = new PuzzleSolver().CalculateSimilarityScore(left, right);
  stopwatch.Stop();
  Console.WriteLine($"The similarity score between the lists is {similarityScore}. ({stopwatch.ElapsedMilliseconds}ms)");
}
else
{
  var totalDistance = new PuzzleSolver().CalculateTotalDistance(left, right);
  stopwatch.Stop();
  Console.WriteLine($"The total distance between the lists is {totalDistance}. ({stopwatch.ElapsedMilliseconds}ms)");
}