using Godot;
using System;

public class DataCollectorTests : WAT.Test
{
    DataCollector DC = new DataCollector(new Godot.Collections.Array());
    public override String Title()
	{
		return "DataCollector Unit Tests";
	}

    [Test]
	public void UpdateStarvationTest() 
	{
        DC.UpdateStarvation();
        Assert.IsEqual(1, DC.GetCurrentStarvation(), "Then it passes");
	}
    [Test]
	public void UpdateDehydrationTest() 
	{

        DC.UpdateDehydration();
        Assert.IsEqual(1, DC.GetCurrentDehydration(), "Then it passes");
	}
    [Test]
    public void UpdateOldAgeTest() 
	{

        DC.UpdateOldAge();
        Assert.IsEqual(1, DC.GetCurrentOldAge(), "Then it passes");
	}
    [Test]
    public void UpdateBeingHuntedTest() 
	{

        DC.UpdateBeingHunted();
        Assert.IsEqual(1, DC.GetCurrentBeingHunted(), "Then it passes");
	}
    [Test]
    public void UpdateHeatStrokeTest() 
	{

        DC.UpdateHeatStroke();
        Assert.IsEqual(1, DC.GetCurrentHeatStroke(), "Then it passes");
	}
    [Test]
    public void UpdateFreezingTest() 
	{

        DC.UpdateFreezing();
        Assert.IsEqual(1, DC.GetCurrentFreezing(), "Then it passes");
	}
    [Test]
    public void UpdateSleepDeprivationTest() 
	{

        DC.UpdateSleepDeprivation();
        Assert.IsEqual(1, DC.GetCurrentSleepDeprivation(), "Then it passes");
	}
    [Test]
    public void ConvertToFloatArrayEmptyArrayTest() 
	{
        Assert.IsEqual(0, DataCollector.ConvertToFloatArray(new Godot.Collections.Array()).Count, "Then it passes");
	}
    [Test]
    public void ConvertToFloatArrayNotEmptyArrayTest() 
	{
        const int X = 5;
        const int Y = 5;

        bool allFloat = true;
        Godot.Collections.Array inputArray = new Godot.Collections.Array();
        for (int i = 0; i < X; i++){
            inputArray.Add((float)i/10.0f);
        }
        Godot.Collections.Array parsed = new Godot.Collections.Array((Godot.Collections.Array)JSON.Parse(JSON.Print(inputArray)).Result);
        Godot.Collections.Array arrayOfFloats = DataCollector.ConvertToFloatArray(parsed);
        for(int i = 0; i < arrayOfFloats.Count; i++){
            if (arrayOfFloats[i].GetType() != typeof(float)){
                allFloat = false;
                break;
            }
        }
        Assert.IsEqual(Y, arrayOfFloats.Count, "Then it passes");
        Assert.IsTrue(allFloat, "Then it passes");
	}
}
