using System.Diagnostics;

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

var instructions = new PuzzleParser().Parse(input);

var results = 0;

stopwatch.Stop();
Console.WriteLine($"The sum of instructions is {results}. ({stopwatch.ElapsedMilliseconds}ms)");

class PuzzleParser
{
  public List<Instruction> Parse(string input)
  {
    return [];
  }
}

class Instruction(int multiplicandOne, int multiplicandTwo)
{
}