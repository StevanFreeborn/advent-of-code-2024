namespace MullItOver;

public static class InstructionExtensions
{
  public static int Execute(this List<Instruction> instructions)
  {
    var isEnabled = true;
    var sum = 0;

    foreach (var instruction in instructions)
    {
      switch (instruction)
      {
        case DontInstruction:
          isEnabled = false;
          continue;
        case DoInstruction:
          isEnabled = true;
          continue;
        case MulInstruction mul when isEnabled:
          sum += mul.Execute();
          continue;
      }
    }
    
    return sum;
  }
}