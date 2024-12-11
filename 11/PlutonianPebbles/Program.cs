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

var stones = input
  .Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
  .Select(Stone.From)
  .ToList();

var observer = Observer.Of(stones);

var result = observer.Blink(25);

stopwatch.Stop();
Console.WriteLine($". ({stopwatch.ElapsedMilliseconds}ms)");

class Observer
{
  private readonly List<Stone> _stones;

  private Observer(List<Stone> stones)
  {
    _stones = stones;
  }

  public static Observer Of(List<Stone> stones) => new(stones);

  public string Blink(int times)
  {
    if (times < 1)
    {
      return string.Join(" ", _stones);
    }
    
    return string.Empty;
  }
}

record Stone
{
  private readonly string _engravedValue;

  private Stone(string engravedValue)
  {
    _engravedValue = engravedValue;
  }

  public static Stone From(string engraving) => new(engraving);

  public List<Stone> Transform()
  {
    if (_engravedValue is "0")
    {
      return [new Stone("1")];
    }

    var hasOddNumberOfDigits = _engravedValue.Length % 2 is not 0;

    if (hasOddNumberOfDigits)
    {
      var engravedNumber = int.Parse(_engravedValue);
      var newEngravedValue = engravedNumber * 2024;
      return [new(newEngravedValue.ToString())];
    }

    var middleIndex = _engravedValue.Length / 2;
    var leftDigits = _engravedValue[..middleIndex].TrimStart('0');
    var rightDigits = _engravedValue[middleIndex..].TrimStart('0');

    return [new(leftDigits), new(rightDigits)];
  }

  public override string ToString()
  {
    return _engravedValue;
  }
}