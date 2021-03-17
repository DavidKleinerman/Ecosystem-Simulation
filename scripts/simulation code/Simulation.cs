using Godot;
using System;

public class Simulation : Spatial
{
	private PackedScene WaterTile = (PackedScene)GD.Load("res://assets/biomes/WaterTile.tscn");
	private PackedScene ForestTile = (PackedScene)GD.Load("res://assets/biomes/ForestTile.tscn");
	private PackedScene DesertTile = (PackedScene)GD.Load("res://assets/biomes/DesertTile.tscn");
	private PackedScene GrasslandTile = (PackedScene)GD.Load("res://assets/biomes/GrasslandTile.tscn");
	private PackedScene TundraTile = (PackedScene)GD.Load("res://assets/biomes/TundraTile.tscn");

	private PackedScene TileSelector = (PackedScene)GD.Load("res://assets/TileSelector.tscn");

	private PackedScene wallCollider = (PackedScene)GD.Load("res://assets/biomes/WallCollider.tscn");

	private Vector3 lastValidCameraPos;
	private Node TileSelectInst;

	private int selectedBiome = 4; //forest = 0, grassland = 1, desert = 2, tundra = 3, water = 4
	private bool isWorldBuilding = true;

	private bool mouseOnList = false;
	//public static Global screenBorder;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//screenBorder = GetNode<Global>("Global");
		// Vector3 position = (Vector3) new Vector3(0,0,0);
		// position.x = -62;
		// position.z = -62;
		// for(int i = 0; i < 32; i++){
		// 	for(int j = 0; j < 32; j++){
		// 		AddTile(WaterTile, position);
		// 		position.z += 4;
		// 	}
		// 	position.x += 4;
		// 	position.z = -62;
		// }
		// TileSelectInst = TileSelector.Instance();
		// AddChild(TileSelectInst);
		OS.SetUseVsync(Global.enableVSync);
		GetNode<DirectionalLight>("DirectionalLight").ShadowEnabled = Global.enableShadows;
	}

	private void _on_StartSimulation_pressed()
	{
		// GetTree().CallGroup("GroundTiles", "RemoveCollider");
		// GetTree().CallGroup("GroundTiles", "StartTimer");
		// GetTree().CallGroup("WaterTiles", "RemoveCollider");
		// GetTree().CallGroup("WaterTiles", "AddWallCollider");
		// TileSelectInst.QueueFree();
		// isWorldBuilding = false;
		// for(int i = 0; i <= 3; i++){
		// 	AddWall(i);
		// }
		/*for (int i = 0; i < 34; i++){
			if(i == 0 || i == 33){
				position.x = -66;
				for (int j = 0; j < 34; j++){
					AddTile(wallCollider, position);
					position.x += 4;
				}
			}
			position.x = -66;
			AddTile(wallCollider, position);
			position.x = 66;
			AddTile(wallCollider, position);
			position.z += 4;
		}*/
	}

	private void AddWall(int side){
		Vector3 position = (Vector3) new Vector3(0,0,0);
		Vector3 scale = (Vector3) new Vector3(2,2,2);
		switch(side){
			case 0:
				position.x = -66;
				position.z = 0;
				scale.z = 64;
			break;
			case 1:
				position.x = 0;
				position.z = 66;
				scale.x = 64;
			break;
			case 2:
				position.x = 0;
				position.z = -66;
				scale.x = 64;
			break;
			case 3:
				position.x = 66;
				position.z = 0;
				scale.z = 64;
			break;
		}
		Node newWall = wallCollider.Instance();
		newWall.RemoveFromGroup("Water");
		((Spatial)newWall).Scale = scale;
		((Spatial)newWall).Translation = position;
		AddChild(newWall);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Vector3 cameraPos = ToGlobal(((Spatial)GetNode("CameraHolder")).Translation);
		RestrictHeight(cameraPos.y);
		RestrictLeftRight(cameraPos.x);
		RestrictFrontBack(cameraPos.z);
	}

	private void RestrictHeight(float height){
		if (height >= 100)
			GetNode<CameraHolder>("CameraHolder").SetAtTop(true);
		else if (height <= 8)
			GetNode<CameraHolder>("CameraHolder").SetAtBottom(true);
		else {
			GetNode<CameraHolder>("CameraHolder").SetAtTop(false);
			GetNode<CameraHolder>("CameraHolder").SetAtBottom(false);
		}
	}

	private void RestrictLeftRight(float xCoordinate){
		if (xCoordinate >= 64)
			GetNode<CameraHolder>("CameraHolder").SetAtRight(true);
		else if (xCoordinate <= -64)
			GetNode<CameraHolder>("CameraHolder").SetAtLeft(true);
		else {
			GetNode<CameraHolder>("CameraHolder").SetAtRight(false);
			GetNode<CameraHolder>("CameraHolder").SetAtLeft(false);
		}
	}

	private void RestrictFrontBack(float zCoordinate){
		if (zCoordinate >= 64)
			GetNode<CameraHolder>("CameraHolder").SetAtBack(true);
		else if (zCoordinate <= -64)
			GetNode<CameraHolder>("CameraHolder").SetAtFront(true);
		else {
			GetNode<CameraHolder>("CameraHolder").SetAtFront(false);
			GetNode<CameraHolder>("CameraHolder").SetAtBack(false);
		}
	}

	// public override void _PhysicsProcess(float delta)
	// {
	// 	if (isWorldBuilding && !mouseOnList)
	// 		SelectBiome();
	// }

	private void _on_ItemList_item_selected(int index)
	{
		selectedBiome = index;
		// GD.Print(index, "\n");
	}

	// private void SelectBiome(){
	// 	Node selectedTile = GetObjectUnderMouse();
	// 	//GD.Print(selectedTile.Name);
	// 	if (selectedTile.Name == "StaticBody"){
	// 		Vector3 selectedPos = ((Spatial)selectedTile).ToGlobal(((Spatial)selectedTile).Translation);
	// 		Vector3 tileSelectorPos = selectedPos;
	// 		tileSelectorPos.y += 1;
	// 		((Spatial)TileSelectInst).Translation = tileSelectorPos;
	// 		if(Input.IsActionPressed("ui_select")){
	// 			ReplaceBiome(selectedPos);
	// 		}
	// 	}

	// }

	// private void ReplaceBiome(Vector3 selectedPos){
	// 	//GD.Print(selectedPos);
	// 	foreach (Node n in GetChildren()){
	// 		if (n.Name != "Control" && n.Name != "SpeciesHolder" && n.Name != "DirectionalLight" && n.Name != "NewSpeciesMenu" && n.Name != "DisplayChartsMenu" && n.Name != "GradientControl"){
	// 			Vector3 position = ((Spatial)n).Translation;
	// 			if (position.x == selectedPos.x && position.y == selectedPos.y && position.z == selectedPos.z){
	// 				GD.Print("Found it!\n");
	// 				n.QueueFree();
	// 				break;
	// 			}
	// 		}
	// 	}
	// 	switch(selectedBiome){
	// 		case 0:
	// 			AddTile(ForestTile, selectedPos);
	// 		break;
	// 		case 1:
	// 			AddTile(GrasslandTile, selectedPos);
	// 		break;
	// 		case 2:
	// 			AddTile(DesertTile, selectedPos);
	// 		break;
	// 		case 3:
	// 			AddTile(TundraTile, selectedPos);
	// 		break;
	// 		case 4:
	// 			AddTile(WaterTile, selectedPos);
	// 		break;
	// 	}
	// }

	// private void AddTile(PackedScene tileType, Vector3 position){
	// 	Node newTileInst = tileType.Instance();
	// 	((Spatial)newTileInst).Translation = position;
	// 	AddChild(newTileInst);
	// }

	// private Node GetObjectUnderMouse(){
	// 	Vector2 mousePos = (Vector2) new Vector2(0, 0);
	// 	mousePos = GetViewport().GetMousePosition();
	// 	Vector3 rayFrom = ToGlobal(((Camera)GetNode("CameraHolder/Camera")).ProjectRayOrigin(mousePos));
	// 	Vector3 rayTo = rayFrom + ToGlobal(((Camera)GetNode("CameraHolder/Camera")).ProjectRayNormal(mousePos)) * 10000;
	// 	PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
	// 	var hit = spaceState.IntersectRay(rayFrom, rayTo);
	// 	Node selection = (Node) new Node();
	// 	if (hit.Contains("collider"))
	// 		selection = (Node)hit["collider"];
	// 	return selection;
	// }

	private void _on_ItemList_mouse_entered()
	{
		mouseOnList = true;
	}

	private void _on_ItemList_mouse_exited()
	{
		mouseOnList = false;
	}

	private void _on_StartSimulation_mouse_entered()
	{
		mouseOnList = true;
	}

	private void _on_StartSimulation_mouse_exited()
	{
		mouseOnList = false;
	}

}

