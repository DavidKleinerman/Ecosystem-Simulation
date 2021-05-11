using Godot;
using System;

public class BiomeGrid : GridMap
{

	public enum BiomeType {
		desert,
		grassland,
		tundra,
		forest
	}
	public class GroundTile : Godot.Reference { //godot has major bugs when using structs. This is a work-around.
		public BiomeType type;
		public Spatial plantSpatial;
		public float plantGrowthTime;
		public int EatersCount;
		public bool isPlantGrowing;
		public bool hasPlant;
		public Vector3 gridIndex;
	}
	private float GlobalGrowthRate;
	private int InitialBiome;
	private int WorldSize;
	private SpatialMaterial ForestMaterial = (SpatialMaterial)GD.Load<SpatialMaterial>("res://materials/forestPlant_material.tres");
	private SpatialMaterial DesertMaterial = (SpatialMaterial)GD.Load<SpatialMaterial>("res://materials/desertPlant_material.tres");
	private SpatialMaterial GrasslandMaterial = (SpatialMaterial)GD.Load<SpatialMaterial>("res://materials/forest_material.tres");
	private SpatialMaterial TundraMaterial = (SpatialMaterial)GD.Load<SpatialMaterial>("res://materials/tundraPlant_material.tres");
	private PackedScene TileSelector = (PackedScene)GD.Load("res://assets/TileSelector.tscn");
	private PackedScene WallCollider = (PackedScene)GD.Load("res://assets/biomes/WallCollider.tscn");
	private PackedScene TileCollider = (PackedScene)GD.Load("res://assets/biomes/TileCollider.tscn");
	private int selectedBiome = 4; //forest = 0, grassland = 1, desert = 2, tundra = 3, water = 4
	private bool mouseOnGUI = false;
	private bool isWorldBuilding = true;
	private Node TileSelectInst;
	private Godot.Collections.Dictionary<Vector3, GroundTile> GroundTiles = (Godot.Collections.Dictionary<Vector3, GroundTile>) new Godot.Collections.Dictionary<Vector3, GroundTile>();
	MultiMeshInstance MultiMeshPlants;
	private const float WaitTime = 5f;
	private float CurrentWaitingTime = 0.0f;
	private float TimeMultiplier = 1f;
	private Godot.Collections.Array PlantBiomassArray = new Godot.Collections.Array();
	public override void _Ready()
	{
		
		GlobalGrowthRate = Global.biomeGrowthRate;
		if (!Global.IsLoaded){
			switch (Global.biomeType){
				case 0:
					InitialBiome = 4;
				break;
				case 1:
					InitialBiome = 0;
				break;
				case 2:
					InitialBiome = 1;
				break;
				case 3:
					InitialBiome = 2;
				break;
				case 4:
					InitialBiome = 3;
				break;
			}
			TileSelectInst = TileSelector.Instance();
			AddChild(TileSelectInst);
		} else {
			InitialBiome = 4;
		}
		WorldSize = Global.worldSize;

		Vector3 position = (Vector3) new Vector3(0,0,0);
		position.x = -WorldSize/2;
		position.z = -WorldSize/2;
		for(int i = 0; i < WorldSize; i++){
			for(int j = 0; j < WorldSize; j++){
				SetCellItem((int)position.x, (int)position.y, (int)position.z, InitialBiome);
				position.z += 1;
			}
			position.x += 1;
			position.z = -WorldSize/2;
		}

		position = (Vector3) new Vector3(0,1,0);
		position.x = -(WorldSize*2 - 2);
		position.z = -(WorldSize*2 - 2);
		for(int i = 0; i < WorldSize; i++){
			for(int j = 0; j < WorldSize; j++){
				Spatial tileCollInst = (Spatial)TileCollider.Instance();
				tileCollInst.Translation = position;
				tileCollInst.Scale = (Vector3) new Vector3(2, 0.7f, 2);
				AddChild(tileCollInst);
				position.z += 4;
			}
			position.x += 4;
			position.z = -(WorldSize*2 - 2);
		}
		MultiMeshPlants = GetParent().GetNode<MultiMeshInstance>("MultiMeshPlants");
		MultiMeshPlants.Multimesh.InstanceCount = 0;
		

		if (Global.IsLoaded){
			int i = 0;
			CurrentWaitingTime = Global.LoadedBiomesWaitingTime;
			GD.Print(CurrentWaitingTime);
			GD.Print("type of tile: " + Global.LoadedTiles[0].GetType());
			MultiMeshPlants.Multimesh.InstanceCount = Global.LoadedTiles.Count;
			foreach (Godot.Collections.Dictionary t in Global.LoadedTiles){
				GroundTile newTile = new GroundTile();
				newTile.type = (BiomeType)((int)((float)t["BiomeType"]));
				newTile.plantSpatial = (Spatial) new Spatial();
				newTile.plantSpatial.Translation = new Vector3((float)t["PlantTranslationX"], (float)t["PlantTranslationY"], (float)t["PlantTranslationZ"]);
				newTile.plantSpatial.Scale = new Vector3((float)t["PlantScaleX"], (float)t["PlantScaleY"], (float)t["PlantScaleZ"]);
				newTile.plantGrowthTime = (float)t["PlantGrowthTime"];
				newTile.EatersCount = (int)((float)t["EatersCount"]);
				newTile.isPlantGrowing = (bool)t["IsPlantGrowing"];
				newTile.hasPlant = (bool)t["HasPlant"];
				newTile.gridIndex = new Vector3((float)t["GridIndexX"], (float)t["GridIndexY"], (float)t["GridIndexZ"]);
				if(newTile.plantSpatial.Scale.x > 0.0f || newTile.hasPlant || newTile.isPlantGrowing){
					SetPlantColor(i, newTile.type);
					MultiMeshPlants.Multimesh.SetInstanceTransform(i, newTile.plantSpatial.Transform);
				}
				GroundTiles.Add(newTile.gridIndex, newTile);
				switch (newTile.type){
					case BiomeType.desert:
						SetCellItem((int)newTile.gridIndex.x, (int)newTile.gridIndex.y, (int)newTile.gridIndex.z, 1);
					break;
					case BiomeType.forest:
						SetCellItem((int)newTile.gridIndex.x, (int)newTile.gridIndex.y, (int)newTile.gridIndex.z, 0);
						break;
					case BiomeType.grassland:
						SetCellItem((int)newTile.gridIndex.x, (int)newTile.gridIndex.y, (int)newTile.gridIndex.z, 2);
						break;
					case BiomeType.tundra:
						SetCellItem((int)newTile.gridIndex.x, (int)newTile.gridIndex.y, (int)newTile.gridIndex.z, 3);
						break;
				}
				i++;
			}
			isWorldBuilding = false;
		}
		
	}

	public Godot.Collections.Array Save(){
		Godot.Collections.Array savedTiles = (Godot.Collections.Array) new Godot.Collections.Array();
		foreach (Vector3 key in GroundTiles.Keys){
			Godot.Collections.Dictionary<String, object> TileDictionary = new Godot.Collections.Dictionary<String, object>() {
				{"BiomeType", (int)(GroundTiles[key].type)},
				{"PlantTranslationX", GroundTiles[key].plantSpatial.Translation.x},
				{"PlantTranslationY", GroundTiles[key].plantSpatial.Translation.y},
				{"PlantTranslationZ", GroundTiles[key].plantSpatial.Translation.z},
				{"PlantScaleX", GroundTiles[key].plantSpatial.Scale.x},
				{"PlantScaleY", GroundTiles[key].plantSpatial.Scale.y},
				{"PlantScaleZ", GroundTiles[key].plantSpatial.Scale.z},
				{"PlantGrowthTime", GroundTiles[key].plantGrowthTime},
				{"EatersCount", GroundTiles[key].EatersCount},
				{"IsPlantGrowing", GroundTiles[key].isPlantGrowing},
				{"HasPlant", GroundTiles[key].hasPlant},
				{"GridIndexX", GroundTiles[key].gridIndex.x},
				{"GridIndexY", GroundTiles[key].gridIndex.y},
				{"GridIndexZ", GroundTiles[key].gridIndex.z},
			};
			savedTiles.Add(TileDictionary);
		}
		return savedTiles;
	}

	// public override void _Process(float delta)
	// {
	// 	int i = 0;
	// 	foreach (Vector3 key in GroundTiles.Keys){
	// 		if (GroundTiles[key].isPlantGrowing){
	// 			Vector3 currentScale = GroundTiles[key].plantSpatial.Scale;
	// 			Vector3 currentTranslation = GroundTiles[key].plantSpatial.Translation;
	// 			if(currentScale.x < 1 && GroundTiles[key].plantGrowthTime < 2.5){
	// 				GroundTiles[key].plantGrowthTime += delta;
	// 				currentScale.x += 0.5f * delta;
	// 				currentScale.y += 0.5f * delta;
	// 				currentScale.z += 0.5f * delta;
	// 				currentTranslation.y += 0.25f * delta;
	// 				GroundTiles[key].plantSpatial.Scale = currentScale;
	// 				GroundTiles[key].plantSpatial.Translation = currentTranslation;
	// 				MultiMeshPlants.Multimesh.SetInstanceTransform(i, GroundTiles[key].plantSpatial.Transform);
	// 			} else {
	// 				GroundTiles[key].plantGrowthTime = 0;
	// 				GroundTiles[key].isPlantGrowing = false;
	// 			}
	// 		}
	// 		i++;
	// 	}
	// }

	public override void _PhysicsProcess(float delta)
	{
		if (isWorldBuilding && !mouseOnGUI)
			SelectBiome();
		else if (!isWorldBuilding){
			CurrentWaitingTime += TimeMultiplier * delta;
			if (CurrentWaitingTime >= WaitTime){
				UpdatePlants();
				CurrentWaitingTime = 0.0f;
			}

			int i = 0;
			foreach (Vector3 key in GroundTiles.Keys){
				if (GroundTiles[key].isPlantGrowing){
					Vector3 currentScale = GroundTiles[key].plantSpatial.Scale;
					Vector3 currentTranslation = GroundTiles[key].plantSpatial.Translation;
					if(currentScale.x < 1 && GroundTiles[key].plantGrowthTime < 2.5f){
						GroundTiles[key].plantGrowthTime += delta;
						currentScale.x += 0.5f * TimeMultiplier * delta;
						currentScale.y += 0.5f * TimeMultiplier * delta;
						currentScale.z += 0.5f * TimeMultiplier * delta;
						currentTranslation.y += 0.25f * TimeMultiplier * delta;
						GroundTiles[key].plantSpatial.Scale = currentScale;
						GroundTiles[key].plantSpatial.Translation = currentTranslation;
						MultiMeshPlants.Multimesh.SetInstanceTransform(i, GroundTiles[key].plantSpatial.Transform);
					} else {
						GroundTiles[key].plantGrowthTime = 0;
						GroundTiles[key].isPlantGrowing = false;
					}
				}
				if (GroundTiles[key].EatersCount> 0){
					Vector3 eatRate = (Vector3) new Vector3(1,1,1);
					Vector3 currentTranslation = GroundTiles[key].plantSpatial.Translation;
					Vector3 oldTranslation = GroundTiles[key].plantSpatial.Translation;
					eatRate *= GroundTiles[key].EatersCount * 0.5f * TimeMultiplier * delta;
					currentTranslation.y -= GroundTiles[key].EatersCount * 0.25f * TimeMultiplier * delta;
					GroundTiles[key].plantSpatial.Scale -= eatRate;
					GroundTiles[key].plantSpatial.Translation = currentTranslation;
					if (GroundTiles[key].plantSpatial.Scale.x < 0.05f){
						GroundTiles[key].plantSpatial.Scale = (Vector3) new Vector3(0.05f, 0.05f, 0.05f);
						GroundTiles[key].plantSpatial.Translation = oldTranslation;
						GroundTiles[key].hasPlant = false;
						GroundTiles[key].plantGrowthTime = 0;
						GroundTiles[key].isPlantGrowing = false;
					}
					MultiMeshPlants.Multimesh.SetInstanceTransform(i, GroundTiles[key].plantSpatial.Transform);
				}
				i++;
			}

		}
	}

	public void UpdatePlantBiomass(){
		float currentPlantBiomass = 0.0f;
		foreach (Vector3 key in GroundTiles.Keys){
			if (GroundTiles[key].isPlantGrowing || GroundTiles[key].hasPlant)
				currentPlantBiomass += GroundTiles[key].plantSpatial.Scale.x;
		}
		PlantBiomassArray.Add(currentPlantBiomass);
	}

	public Godot.Collections.Array GetPlantBiomassArray(){
		return PlantBiomassArray;
	}

	private void SelectBiome(){
		Node selectedTile = GetObjectUnderMouse();
		if (selectedTile.IsInGroup("TileColliders")){
			Vector3 selectedPos = ToLocal(((Spatial)selectedTile).Translation);
			selectedPos.y += 1;
			((Spatial)TileSelectInst).Translation = selectedPos;
			selectedPos.y -= 1;
			if(Input.IsActionPressed("ui_select")){
				ReplaceBiome(selectedPos);
			}
		}

	}

	private void ReplaceBiome(Vector3 selectedPos){
		switch(selectedBiome){
			case 0:
				SetCellItem((int)WorldToMap(selectedPos).x, (int)WorldToMap(selectedPos).y, (int)WorldToMap(selectedPos).z, 0);
			break;
			case 1:
				SetCellItem((int)WorldToMap(selectedPos).x, (int)WorldToMap(selectedPos).y, (int)WorldToMap(selectedPos).z, 2);
			break;
			case 2:
				SetCellItem((int)WorldToMap(selectedPos).x, (int)WorldToMap(selectedPos).y, (int)WorldToMap(selectedPos).z, 1);
			break;
			case 3:
				SetCellItem((int)WorldToMap(selectedPos).x, (int)WorldToMap(selectedPos).y, (int)WorldToMap(selectedPos).z, 3);
			break;
			case 4:
				SetCellItem((int)WorldToMap(selectedPos).x, (int)WorldToMap(selectedPos).y, (int)WorldToMap(selectedPos).z, 4);
			break;
		}
	}

	private void AddTileToArray(BiomeType tileType, Vector3 position){
		GroundTile newTile = new GroundTile();
		newTile.type = tileType;
		newTile.plantSpatial = (Spatial) new Spatial();
		newTile.plantGrowthTime = 0f;
		newTile.EatersCount = 0;
		newTile.isPlantGrowing = false;
		newTile.hasPlant = false;
		newTile.gridIndex = position;
		GroundTiles.Add(newTile.gridIndex, newTile);		
	}

	private Node GetObjectUnderMouse(){
		Vector2 mousePos = (Vector2) new Vector2(0, 0);
		mousePos = GetViewport().GetMousePosition();
		Vector3 rayFrom = ToGlobal(((Camera)GetNode("../CameraHolder/Camera")).ProjectRayOrigin(mousePos));
		Vector3 rayTo = rayFrom + ToGlobal(((Camera)GetNode("../CameraHolder/Camera")).ProjectRayNormal(mousePos)) * 10000;
		PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
		var hit = spaceState.IntersectRay(rayFrom, rayTo);
		Node selection = (Node) new Node();
		if (hit.Contains("collider"))
			selection = (Node)hit["collider"];
		return selection;
	}

	private void _on_ItemList_item_selected(int index)
	{
		selectedBiome = index;
		GD.Print(index, "\n");
	}


	private void _on_ItemList_mouse_entered()
	{
		mouseOnGUI = true;
	}


	private void _on_ItemList_mouse_exited()
	{
		mouseOnGUI = false;
	}


	private void _on_StartSimulation_mouse_entered()
	{
		mouseOnGUI = true;
	}


	private void _on_StartSimulation_mouse_exited()
	{
		mouseOnGUI = false;
	}

	private void _on_StartSimulation_pressed()
	{
		isWorldBuilding = false;
		Godot.Collections.Array children = GetChildren();
		foreach (Node c in children){
			c.QueueFree();
		}
		Vector3 position = (Vector3) new Vector3(0,0,0);
		position.x = -WorldSize/2;
		position.z = -WorldSize/2;
		for(int i = 0; i < WorldSize; i++){
			for(int j = 0; j < WorldSize; j++){
				switch(GetCellItem((int)position.x, (int)position.y, (int)position.z)){
					case 0:
						AddTileToArray(BiomeType.forest, position);
					break;
					case 1:
						AddTileToArray(BiomeType.desert, position);
					break;
					case 2:
						AddTileToArray(BiomeType.grassland, position);
					break;
					case 3:
						AddTileToArray(BiomeType.tundra, position);
					break;
				}
				position.z += 1;
			}
			position.x += 1;
			position.z = -WorldSize/2;
		}
		MultiMeshPlants.Multimesh.InstanceCount = GroundTiles.Count;
		PlantBiomassArray.Add(0.0f);
	}

	private float PlantChance(){
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		return rng.RandfRange(0f, 100f);
	}

	private float TotalGrowRate(BiomeType biomeType){
		switch(biomeType){
			case BiomeType.desert:
				return 0.57f * GlobalGrowthRate;
				break;
			case BiomeType.forest:
				return 17.92f * GlobalGrowthRate;
				break;
			case BiomeType.grassland:
				return 1.19f * GlobalGrowthRate;
				break;
			default:
				return 0.18f * GlobalGrowthRate;
				break;
		}
	}

	private void SetPlantColor(int inst, BiomeType type){
		switch(type){
			case BiomeType.desert:
				MultiMeshPlants.Multimesh.SetInstanceColor(inst, DesertMaterial.AlbedoColor);
				break;
			case BiomeType.forest:
				MultiMeshPlants.Multimesh.SetInstanceColor(inst, ForestMaterial.AlbedoColor);
				break;
			case BiomeType.grassland:
				MultiMeshPlants.Multimesh.SetInstanceColor(inst, GrasslandMaterial.AlbedoColor);
				break;
			case BiomeType.tundra:
				MultiMeshPlants.Multimesh.SetInstanceColor(inst, TundraMaterial.AlbedoColor);
				break;
		}
	}
	
	private void UpdatePlants()
	{
		int i = 0;
		foreach (Vector3 key in GroundTiles.Keys){
			if(!GroundTiles[key].hasPlant){
				if (PlantChance() < TotalGrowRate(GroundTiles[key].type)){
					GroundTiles[key].plantSpatial.Translation = MapToWorld((int)GroundTiles[key].gridIndex.x, (int)GroundTiles[key].gridIndex.y + 1, (int)GroundTiles[key].gridIndex.z);
					GroundTiles[key].plantSpatial.Translation = new Vector3(GroundTiles[key].plantSpatial.Translation.x, GroundTiles[key].plantSpatial.Translation.y - 1.2f, GroundTiles[key].plantSpatial.Translation.z);
					GroundTiles[key].plantSpatial.Scale = new Vector3();
					MultiMeshPlants.Multimesh.SetInstanceTransform(i, GroundTiles[key].plantSpatial.Transform);
					SetPlantColor(i, GroundTiles[key].type);
					GroundTiles[key].hasPlant = true;
					GroundTiles[key].isPlantGrowing = true;
				}
			} else if (GroundTiles[key].plantSpatial.Scale.x < 0.4f){
				if (PlantChance() < TotalGrowRate(GroundTiles[key].type)){
					GroundTiles[key].hasPlant = true;
					GroundTiles[key].isPlantGrowing = true;
				}
			}
			i++;
		}
	}

	public Godot.Collections.Dictionary<Vector3, GroundTile> GetGroundTiles(){
		return GroundTiles;
	}

	private void _on_SimulationRate_item_selected(int index)
	{
		switch (index){
			case 0:
				TimeMultiplier = 1f;
			break;
			case 1:
				TimeMultiplier = 2f;
			break;
			case 2:
				TimeMultiplier = 4f;
			break;
		}
	}

	public float GetCurrentWaitingTime(){
		return CurrentWaitingTime;
	}
}








