using System.Text.RegularExpressions;

namespace ClawContraption;

partial class Machine
{
  private readonly int? _maxPresses;
  private readonly Button _buttonA;
  private readonly Button _buttonB;
  private readonly PrizeLocation _prizeLocation;
  private decimal NumberOfButtonBPresses => 
    ((_prizeLocation.Y * _buttonA.XMovement) - (_prizeLocation.X * _buttonA.YMovement)) / ((_buttonB.YMovement * _buttonA.XMovement) - (_buttonB.XMovement * _buttonA.YMovement));

  private decimal NumberOfButtonAPresses =>
    (_prizeLocation.X - (_buttonB.XMovement * NumberOfButtonBPresses)) / _buttonA.XMovement;

  private bool HasSolution => CheckForSolution();

  public decimal TokenCost => HasSolution ? (NumberOfButtonBPresses * 1) + (NumberOfButtonAPresses * 3) : 0;
  
  private Machine(Button buttonA, Button buttonB, PrizeLocation prizeLocation, int? maxPresses = null)
  {
    _buttonA = buttonA;
    _buttonB = buttonB;
    _prizeLocation = prizeLocation;
    _maxPresses = maxPresses;
  }

  internal static Machine From(Button buttonA, Button buttonB, PrizeLocation prizeLocation, int? maxPresses) =>
    new(buttonA, buttonB, prizeLocation, maxPresses);

  public static Machine From(string[] input, int? maxPresses = null, long prizeLocationOffset = 0)
  {
    if (input.Length is not 3)
    {
      throw new ApplicationException("Input describing machine is not valid");
    }

    var buttonAInput = input[0];
    var buttonBInput = input[1];
    var prizeInput = input[2];
    
    var buttonAMatches = ButtonARegex().Match(buttonAInput);
    var buttonBMatches = ButtonBRegex().Match(buttonBInput);
    var prizeMatches = PrizeRegex().Match(prizeInput);

    if (buttonAMatches.Groups.Count is not 3)
    {
      throw new ApplicationException("input for button A missing values");
    }
    
    if (buttonBMatches.Groups.Count is not 3)
    {
      throw new ApplicationException("input for button B missing values");
    }
    
    if (prizeMatches.Groups.Count is not 3)
    {
      throw new ApplicationException("input for prize missing values");
    }

    var buttonA = new Button(
      int.Parse(buttonAMatches.Groups[1].Value),
      int.Parse(buttonAMatches.Groups[2].Value)
    );
    
    var buttonB = new Button(
      int.Parse(buttonBMatches.Groups[1].Value),
      int.Parse(buttonBMatches.Groups[2].Value)
    );
    
    var prize = new PrizeLocation(
      int.Parse(prizeMatches.Groups[1].Value) + prizeLocationOffset,
      int.Parse(prizeMatches.Groups[2].Value) + prizeLocationOffset
    );

    return new(buttonA, buttonB, prize, maxPresses);
  }

  private bool CheckForSolution()
  {
    if (Math.Floor(NumberOfButtonAPresses) != NumberOfButtonAPresses)
    {
      return false;
    }

    if (Math.Floor(NumberOfButtonBPresses) != NumberOfButtonBPresses)
    {
      return false;
    }

    if (_maxPresses.HasValue && NumberOfButtonAPresses > _maxPresses)
    {
      return false;
    }

    if (_maxPresses.HasValue && NumberOfButtonBPresses > _maxPresses)
    {
      return false;
    }

    return true;
  }
  
  [GeneratedRegex(@"Button A: X\+(\d+), Y\+(\d+)")]
  private static partial Regex ButtonARegex();
  [GeneratedRegex(@"Button B: X\+(\d+), Y\+(\d+)")]
  private static partial Regex ButtonBRegex();
  [GeneratedRegex(@"Prize: X=(\d+), Y=(\d+)")]
  private static partial Regex PrizeRegex();
}