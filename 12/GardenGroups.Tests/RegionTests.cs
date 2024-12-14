namespace GardenGroups.Tests;

public class RegionTests
{
  [Test]
  public async Task Region_WhenInstanceConstructed_ItShouldSetPlantTypeProperty()
  {
    var plantType = 'A';

    var region = Region.From('A');

    await Assert.That(region.PlantType).IsEqualTo(plantType);
  }

  [Test]
  public async Task IncreasePerimeter_WhenCalled_ItShouldIncreaseRegionPerimeter()
  {
    var region = Region.From('A');
    
    region.IncreasePerimeter();

    await Assert.That(region.Perimeter).IsEqualTo(1);
  }
  
  [Test]
  public async Task CalculatePrice_WhenCalled_ItShouldReturnExpectedValue()
  {
    var region = Region.From('A');

    var result = region.CalculateFencePriceBasedOnPerimeter();

    await Assert.That(result).IsEqualTo(0);
  }

  [Test]
  public async Task AddPosition_WhenCalled_ItShouldAddAPosition()
  {
    var region = Region.From('A');
    
    region.AddPosition(new(0, 0, 'A'));

    await Assert.That(region.Positions).IsEquivalentTo(new HashSet<Position>() { new(0, 0, 'A') });
    await Assert.That(region.Area).IsEqualTo(1);
  }
}