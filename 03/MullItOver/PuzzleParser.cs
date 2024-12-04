using System.Text.RegularExpressions;

namespace MullItOver;

partial class PuzzleParser
{
  [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
  private static partial Regex InstructionsRegex();
  
  [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)|do\(\)|don't\(\)")]
  private static partial Regex InstructionsWithConditionalsRegex();
  
  public List<Instruction> Parse(string input)
  {
    var matches = InstructionsRegex().Matches(input);
    return matches
      .Select(Instruction (m) => new MulInstruction(
        int.Parse(m.Groups[1].Value), 
        int.Parse(m.Groups[2].Value)
      ))
      .ToList();
  }
  
  public List<Instruction> ParseWithConditionals(string input)
  {
    var instructions = new List<Instruction>();
    var matches = InstructionsWithConditionalsRegex()
      .Matches(input)
      .ToList();

    foreach (var match in matches)
    {
      var firstGroup = match.Groups[0].Value;
      
      if (firstGroup.Contains("mul"))
      {
        instructions.Add(new MulInstruction(
          int.Parse(match.Groups[1].Value), 
          int.Parse(match.Groups[2].Value)
        ));
        continue;
      }

      if (firstGroup.Contains('\''))
      {
        instructions.Add(new DontInstruction());
        continue;
      }
      
      instructions.Add(new DoInstruction());
    }

    return instructions;
  }
}