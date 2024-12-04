namespace MullItOver;

public abstract class Instruction {};

class MulInstruction(int multiplicandOne, int multiplicandTwo) : Instruction
{
  public int Execute() => multiplicandOne * multiplicandTwo;
}

class DontInstruction : Instruction;

class DoInstruction : Instruction;