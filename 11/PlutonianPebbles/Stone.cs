namespace PlutonianPebbles;

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
      var engravedNumber = long.Parse(_engravedValue);
      var newEngravedValue = engravedNumber * 2024;
      return [new(newEngravedValue.ToString())];
    }

    var middleIndex = _engravedValue.Length / 2;
    var leftDigits = long.Parse(_engravedValue[..middleIndex]);
    var rightDigits = long.Parse(_engravedValue[middleIndex..]);
    
    return [new(leftDigits.ToString()), new(rightDigits.ToString())];
  }

  public override string ToString()
  {
    return _engravedValue;
  }
}