namespace DiskFragmenter.Tests;

public class DiskMapTests
{
  private const string TestInput = "12345";
  private const string ExampleInput = "2333133121414131402";

  private async Task<string> GetPuzzleInput()
  {
    return await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "INPUT.txt"));
  }
  
  [Test]
  public async Task FromDenseFormat_WhenCalled_ItShouldReturnDiskMap()
  {
    var map = DiskMap.FromDenseFormat(TestInput);

    await Assert.That(map).IsTypeOf<DiskMap>();
  }

  [Test]
  public async Task CompactDisk_WhenCalled_ItShouldReturnNewDiskCompacted()
  {
    var map = DiskMap.FromDenseFormat(TestInput);
    var compactedMap = map.CompactDisk();

    await Assert.That(compactedMap).IsNotEqualTo(map);
  }
  
  [Test]
  public async Task CompactDiskWithoutFragmentation_WhenCalledWithTestInput_ItShouldReturnNewDiskCompacted()
  {
    var map = DiskMap.FromDenseFormat(TestInput);
    var compactedMap = map.CompactDiskWithoutFragmentation();

    await Assert.That(compactedMap).IsNotEqualTo(map);
  }
  
  [Test]
  public async Task CompactDiskWithoutFragmentation_WhenCalledWithExampleInput_ItShouldReturnNewDiskCompacted()
  {
    var map = DiskMap.FromDenseFormat(ExampleInput);
    var compactedMap = map.CompactDiskWithoutFragmentation();

    await Assert.That(compactedMap).IsNotEqualTo(map);
  }

  [Test]
  public async Task CalculateCheckSum_WhenCalledWhenCalledWithTestInput_ItShouldReturnExpectedValue()
  {
    var result = DiskMap.FromDenseFormat(TestInput)
      .CompactDisk()
      .CalculateCheckSum();

    await Assert.That(result).IsEqualTo(60);
  }

  [Test]
  public async Task CalculateCheckSum_WhenCalledWithExampleInput_ItShouldReturnExpectedValue()
  {
    var result = DiskMap.FromDenseFormat(ExampleInput)
      .CompactDisk()
      .CalculateCheckSum();

    await Assert.That(result).IsEqualTo(1928);
  }
  
  [Test]
  public async Task CalculateCheckSum_WhenCalledWithPuzzleInput_ItShouldReturnExpectedValue()
  {
    var input = await GetPuzzleInput();
    
    var result = DiskMap.FromDenseFormat(input)
      .CompactDisk()
      .CalculateCheckSum();

    await Assert.That(result).IsEqualTo(6200294120911);
  }
  
  [Test]
  public async Task CalculateCheckSum_WhenCalledWhenCalledWithTestInputAndNoFragmentation_ItShouldReturnExpectedValue()
  {
    var result = DiskMap.FromDenseFormat(TestInput)
      .CompactDiskWithoutFragmentation()
      .CalculateCheckSum();

    await Assert.That(result).IsEqualTo(132);
  }

  [Test]
  public async Task CalculateCheckSum_WhenCalledWithExampleInputAndNoFragmentation_ItShouldReturnExpectedValue()
  {
    var result = DiskMap.FromDenseFormat(ExampleInput)
      .CompactDiskWithoutFragmentation()
      .CalculateCheckSum();

    await Assert.That(result).IsEqualTo(2858);
  }
  
  [Test]
  public async Task CalculateCheckSum_WhenCalledWithPuzzleInputAndNoFragmentation_ItShouldReturnExpectedValue()
  {
    var input = await GetPuzzleInput();
    
    var result = DiskMap.FromDenseFormat(input)
      .CompactDiskWithoutFragmentation()
      .CalculateCheckSum();

    await Assert.That(result).IsEqualTo(6227018762750);
  }
}