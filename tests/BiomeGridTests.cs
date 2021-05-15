using Godot;
using System;

public class BiomeGridTests : WAT.Test
{
	public override String Title()
	{
		return "BiomeGrid Unit Tests";
	}
	
	[Test]
	public void UpdatePlantBiomassEmptyDictionary() 
	{
		BiomeGrid bg = new BiomeGrid();
		bg.UpdatePlantBiomass();
		Assert.IsEqual(0.0f, bg.GetPlantBiomassArray()[0], "Then it passes"); 
	}

	[Test]
	public void UpdatePlantBiomassOneElementDictionaryWholeNumbers(){
		int Y = 1;
		BiomeGrid bg = new BiomeGrid();
		BiomeGrid.GroundTile tile = new BiomeGrid.GroundTile();
		tile.plantSpatial = new Spatial();
		tile.plantSpatial.Scale = new Vector3(Y, 0, 0);
		tile.hasPlant = true;
		bg.GetGroundTiles().Add(new Vector3 (0,0,0), tile);
		bg.UpdatePlantBiomass();
		Assert.IsEqual(1f, bg.GetPlantBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void UpdatePlantBiomassOneElementDictionaryFractions(){
		float Y = 0.3f;
		BiomeGrid bg = new BiomeGrid();
		BiomeGrid.GroundTile tile = new BiomeGrid.GroundTile();
		tile.plantSpatial = new Spatial();
		tile.plantSpatial.Scale = new Vector3(Y, 0, 0);
		tile.hasPlant = true;
		bg.GetGroundTiles().Add(new Vector3 (0,0,0), tile);
		bg.UpdatePlantBiomass();
		Assert.IsEqual(0.3f, bg.GetPlantBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void UpdatePlantBiomassMultiElementDictionaryWholeNumbers(){
		int X = 3;
		int Y = 1;
		
		BiomeGrid bg = new BiomeGrid();
		for (int i = 0; i < X; i++){
			BiomeGrid.GroundTile tile = new BiomeGrid.GroundTile();
			tile.plantSpatial = new Spatial();
			tile.plantSpatial.Scale = new Vector3(Y, 0, 0);
			tile.hasPlant = true;
			bg.GetGroundTiles().Add(new Vector3 (i,0,0), tile);
		}
		bg.UpdatePlantBiomass();
		Assert.IsEqual(3f, bg.GetPlantBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void UpdatePlantBiomassMultiElementDictionaryFractions(){
		int X = 3;
		float Y = 0.3f;
		float Z = 0.3f + 0.3f + 0.3f;
		BiomeGrid bg = new BiomeGrid();
		for (int i = 0; i < X; i++){
			BiomeGrid.GroundTile tile = new BiomeGrid.GroundTile();
			tile.plantSpatial = new Spatial();
			tile.plantSpatial.Scale = new Vector3(Y, 0, 0);
			tile.hasPlant = true;
			bg.GetGroundTiles().Add(new Vector3 (i,0,0), tile);
		}
		bg.UpdatePlantBiomass();
		Assert.IsEqual(Z, bg.GetPlantBiomassArray()[0], "Then it passes");
	}
	[Test]
	public void SaveFromEmptyArray(){
		BiomeGrid bg = new BiomeGrid();
		Assert.IsEqual(0, bg.Save().Count, "Then it passes");
	}
	[Test]
	public void SaveFromNotEmptyArray(){
		const int X = 1;
		const int Y = 14;
		BiomeGrid bg = new BiomeGrid();
		BiomeGrid.GroundTile newTile = new BiomeGrid.GroundTile();
		newTile.type = BiomeGrid.BiomeType.grassland;
		newTile.plantSpatial = (Spatial) new Spatial();
		newTile.plantGrowthTime = 0f;
		newTile.EatersCount = 0;
		newTile.isPlantGrowing = false;
		newTile.hasPlant = false;
		newTile.gridIndex = new Vector3(0,0,0);
		bg.GetGroundTiles().Add(newTile.gridIndex, newTile);
		Assert.IsEqual(X, bg.Save().Count, "Then it passes");
		Assert.IsTrue(bg.Save()[0] is Godot.Collections.Dictionary, "Then it passes");
		Assert.IsEqual(Y, ((Godot.Collections.Dictionary)bg.Save()[0]).Count, "Then it passes");
	}

}
