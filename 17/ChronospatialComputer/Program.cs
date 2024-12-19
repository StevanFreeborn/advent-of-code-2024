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

var result = Computer.From(input).RunProgram();

stopwatch.Stop();
Console.WriteLine($"The output of the program is {result}. ({stopwatch.ElapsedMilliseconds}ms)");

class Computer
{
  private readonly int[] _program;
  private readonly Dictionary<int, Func<int, bool>> _instructions;
  
  private int _aRegister;
  private int _bRegister;
  private int _cRegister;
  private int _instructionPointer;
  private readonly List<int> _output = [];

  private Computer(int a, int b, int c, int[] program)
  {
    _aRegister = a;
    _bRegister = b;
    _cRegister = c;
    _program = program;
    
    _instructions = new()
    {
      { 0, Adv },
      { 1, Bxl },
      { 2, Bst },
      { 3, Jnz },
      { 4, Bxc },
      { 5, Out },
      { 6, Bdv },
      { 7, Cdv },
    };
  }

  public static Computer From(string input)
  {
    var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}");

    if (parts.Length is not 2)
    {
      throw new ArgumentException("The computer input has more than 2 parts");
    }

    var registersPart = parts[0];
    var programPart = parts[1];

    var registers = registersPart.Split(Environment.NewLine);

    if (registers.Length is not 3)
    {
      throw new ArgumentException($"There were only {registers.Length} registers given");
    }

    var registerA = int.Parse(registers[0].Split(':')[1].Trim());
    var registerB = int.Parse(registers[1].Split(':')[1].Trim());
    var registerC = int.Parse(registers[2].Split(':')[1].Trim());
    
    var program = programPart.Split(':')[1].Trim().Split(',').Select(int.Parse).ToArray();

    return new(registerA, registerB, registerC, program);
  }

  public string RunProgram()
  {
    while (_instructionPointer < _program.Length)
    {
      var opcode = _program[_instructionPointer];
      var operand = _program[_instructionPointer + 1];

      if (_instructions.TryGetValue(opcode, out var instruction) is false)
      {
        throw new InvalidOperationException($"{nameof(opcode)} is unknown: {opcode}");
      }

      var shouldAdvance = instruction(operand);

      if (shouldAdvance is false)
      {
        continue;
      }
      
      _instructionPointer += 2;
    }

    return string.Join(',', _output);
  }

  private int GetComboOperand(int operand) => operand switch
  {
    0 or 1 or 2 or 3 => operand,
    4 => _aRegister,
    5 => _bRegister,
    6 => _cRegister,
    _ => throw new ArgumentException($"{nameof(operand)} is not a valid operand: {operand}"),
  };
  
  private bool Adv(int operand)
  {
    _aRegister /= (int)Math.Pow(2, GetComboOperand(operand));
    return true;
  }

  private bool Bxl(int operand)
  {
    _bRegister ^= operand;
    return true;
  }

  private bool Bst(int operand)
  {
    _bRegister = GetComboOperand(operand) % 8;
    return true;
  }

  private bool Jnz(int operand)
  {
    if (_aRegister is 0)
    {
      return true;
    }

    _instructionPointer = operand;
    return false;

  }

  private bool Bxc(int operand)
  {
    _bRegister ^= _cRegister;
    return true;
  }

  private bool Out(int operand)
  {
    _output.Add(GetComboOperand(operand) % 8);
    return true;
  }

  private bool Bdv(int operand)
  {
    _bRegister = _aRegister / (int)Math.Pow(2, GetComboOperand(operand));
    return true;
  }

  private bool Cdv(int operand)
  {
    _cRegister = _aRegister / (int)Math.Pow(2, GetComboOperand(operand));
    return true;
  }
}