namespace BridgeRepair;

// TODO: Look at what the public API should be here.
class Equation(long testValue, List<long> numbers)
{
  public readonly long TestValue = testValue;

  public bool IsPossible(bool isPart2 = false)
  {
    var configs = GeneratePossibleOperatorConfigurations(isPart2).ToList();
    return configs.Select(Evaluate).Any(result => result == TestValue);
  }

  public long Evaluate(char[] operators)
  {
    var result = numbers[0];

    for (var i = 0; i < operators.Length; i++)
    {
      var currentOperator = operators[i];
      var number = numbers[i + 1];

      if (currentOperator is '+')
      {
        result += number;
        continue;
      }

      if (currentOperator is '*')
      {
        result *= number;
        continue;
      }

      result = long.Parse(result + number.ToString());
    }
    
    return result;
  }
  
  public IEnumerable<char[]> GeneratePossibleOperatorConfigurations(bool isPart2 = false)
  {
    // TODO: make it possible to pass is operators we can use.
    var numberOfPositions = numbers.Count - 1;
    var config = new char[numberOfPositions];
    var numOfOperators = isPart2 ? 3 : 2;
    var numberOfPossibleConfigurations = Math.Pow(numOfOperators, numberOfPositions);

    for (var i = 0; i < numberOfPossibleConfigurations; i++)
    {
      var j = 0;
      
      while (j < numberOfPositions)
      {
        var currentOperator = config[j];
        
        if (currentOperator is '+')
        {
          config[j] = '*';
          break;
        }

        if (isPart2 && currentOperator is '*')
        {
          config[j] = '|';
          break;
        }

        config[j] = '+';
        j++;
      }
      
      yield return config.ToArray();
    }
  }
}