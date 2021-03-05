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
	public class GroundTile : Godot.Object { //godot has major bugs when using structs. This is a workaround.
		public BiomeType type;
		public Spatial plantSpatial;
		public float plantGrowthTime;
		public int EatersCount;
		public bool isPlantGrowing;
		public bool hasPlant;
		public Vector3 gridIndex;
	}
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
	private Godot.Collections.Array<GroundTile> GroundTiles = (Godot.Collections.Array<GroundTile>) new Godot.Collections.Array<GroundTile>();
	MultiMeshInstance MultiMeshPlants;
	public override void _Ready()
	{
		Vector3 position = (Vector3) new Vector3(0,0,0);
		position.x = -16;
		position.z = -16;
		for(int i = 0; i < 32; i++){
			for(int j = 0; j < 32; j++){
				SetCellItem((int)position.x, (int)position.y, (int)position.z, 4);
				position.z += 1;
			}
			position.x += 1;
			position.z = -16;
		}

		position = (Vector3) new Vector3(0,1,0);
		position.x = -62;
		position.z = -62;
		for(int i = 0; i < 32; i++){
			for(int j = 0; j < 32; j++){
				Spatial tileCollInst = (Spatial)TileCollider.Instance();
				tileCollInst.Translation = position;
				tileCollInst.Scale = (Vector3) new Vector3(2, 0.7f, 2);
				AddChild(tileCollInst);
				position.z += 4;
			}
			position.x += 4;
			position.z = -62;
		}
		TileSelectInst = TileSelector.Instance();
		AddChild(TileSelectInst);
		MultiMeshPlants = GetParent().GetNode<MultiMeshInstance>("MultiMeshPlants");
	}

	public override void _Process(float delta)
	{
		for (int i = 0; i < GroundTiles.Count; i++){
			if (GroundTiles[i].isPlantGrowing){
				Vector3 currentScale = GroundTiles[i].plantSpatial.Scale;
				Vector3 currentTranslation = GroundTiles[i].plantSpatial.Translation;
				if(currentScale.x < 1 && GroundTiles[i].plantGrowthTime < 2.5){
					GroundTiles[i].plantGrowthTime += delta;
					currentScale.x += 0.5f * delta;
					currentScale.y += 0.5f * delta;
					currentScale.z += 0.5f * delta;
					currentTranslation.y += 0.25f * delta;
					GroundTiles[i].plantSpatial.Scale = currentScale;
					GroundTiles[i].plantSpatial.Translation = currentTranslation;
					MultiMeshPlants.Multimesh.SetInstanceTransform(i, GroundTiles[i].plantSpatial.Transform);
				} else {
					GroundTiles[i].plantGrowthTime = 0;
					GroundTiles[i].isPlantGrowing = false;
				}
			}
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		if (isWorldBuilding && !mouseOnGUI)
			SelectBiome();
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
		GroundTiles.Add(newTile);		
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
		position.x = -16;
		position.z = -16;
		for(int i = 0; i < 32; i++){
			for(int j = 0; j < 32; j++){
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
			position.z = -16;
		}
		MultiMeshPlants.Multimesh.InstanceCount = GroundTiles.Count;
		GetParent().GetNode<Timer>("PlantGrowthTimer").Start();
	}

	private float PlantChance(){
		RandomNumberGenerator rng = new RandomNumberGenerator();
		rng.Randomize();
		return rng.RandfRange(0f, 100f);
	}

	private float TotalGrowRate(BiomeType biomeType){
		switch(biomeType){
			case BiomeType.desert:
				return 0.57f * 5;
				break;
			case BiomeType.forest:
				return 17.92f * 5;
				break;
			case BiomeType.grassland:
				return 1.19f * 5;
				break;
			default:
				return 0.18f * 5;
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
	
	private void _on_PlantGrowthTimer_timeout()
	{
		for(int i = 0; i < GroundTiles.Count; i++){
			if(!GroundTiles[i].hasPlant){
				if (PlantChance() < TotalGrowRate(GroundTiles[i].type)){
					//GD.Print("instance count: " + MultiMeshPlants.Multimesh.InstanceCount);
					GroundTiles[i].plantSpatial.Translation = MapToWorld((int)GroundTiles[i].gridIndex.x, (int)GroundTiles[i].gridIndex.y + 1, (int)GroundTiles[i].gridIndex.z);
					GroundTiles[i].plantSpatial.Translation = new Vector3(GroundTiles[i].plantSpatial.Translation.x, GroundTiles[i].plantSpatial.Translation.y - 1.2f, GroundTiles[i].plantSpatial.Translation.z);
					//GD.Print("transfrom: " + GroundTiles[i].plantSpatial.Transform);
					GroundTiles[i].plantSpatial.Scale = new Vector3();
					MultiMeshPlants.Multimesh.SetInstanceTransform(i, GroundTiles[i].plantSpatial.Transform);
					SetPlantColor(i, GroundTiles[i].type);
					GroundTiles[i].hasPlant = true;
					GroundTiles[i].isPlantGrowing = true;
				}
			} else if (GroundTiles[i].plantSpatial.Scale.x < 0.4f){
				if (PlantChance() < TotalGrowRate(GroundTiles[i].type)){
					GroundTiles[i].isPlantGrowing = true;
				}
			}
			// else if (gt.plantSpatial.Scale.x < 0.4){
			// 	if (PlantChance() < TotalGrowRate(gt.type)){
			// 		gt.isPlantGrowing = true;
			// 	}
			// }
		}

		// for (int i = 0; i < MultiMeshPlants.Multimesh.InstanceCount; i++){
		// 	GD.Print("this is my transform: " + MultiMeshPlants.Multimesh.GetInstanceTransform(i));
		// }
	}



}




