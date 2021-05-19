using Godot;
using System;

public class SpeciesTests : WAT.Test
{
    public override String Title()
	{
		return "Species Unit Tests";
	}

    [Test]
	public void InitSpeciesEmptyValuesTest() 
	{
        Species newSpecies = new Species();
        newSpecies.InitSpecies("", new Color(0,0,0), new Godot.Collections.Array(), 0, false, null, Species.GraphicModel.Cube);
        Assert.IsEqual("", newSpecies.GetSpeciesName(), "Then it passes");
        Assert.IsTrue(newSpecies.GetDataCollector() != null, "Then it passes");
        Assert.IsEqual(new Color(0,0,0), newSpecies.GetSpeciesColor(), "Then it passes");
        Assert.IsEqual(Species.Diet.Herbivore, newSpecies.GetSpeciesDiet(), "Then it passes");
        Assert.IsEqual(Species.GraphicModel.Cube, newSpecies.GetSpeciesModel(), "Then it passes");
	}

    [Test]
    public void InitSpeciesNotEmptyValuesTest() 
	{
        Species newSpecies = new Species();
        newSpecies.InitSpecies("Name", new Color(0.5f,0.5f,0.5f), new Godot.Collections.Array(), 2, false, null, Species.GraphicModel.Oval);
        Assert.IsEqual("Name", newSpecies.GetSpeciesName(), "Then it passes");
        Assert.IsTrue(newSpecies.GetDataCollector() != null, "Then it passes");
        Assert.IsEqual(new Color(0.5f,0.5f,0.5f), newSpecies.GetSpeciesColor(), "Then it passes");
        Assert.IsEqual(Species.Diet.CarnivorePredator, newSpecies.GetSpeciesDiet(), "Then it passes");
        Assert.IsEqual(Species.GraphicModel.Oval, newSpecies.GetSpeciesModel(), "Then it passes");
	}
}
