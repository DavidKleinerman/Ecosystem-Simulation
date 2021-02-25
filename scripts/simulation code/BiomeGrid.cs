using Godot;
using System;

public class BiomeGrid : GridMap
{
	private PackedScene TileSelector = (PackedScene)GD.Load("res://assets/TileSelector.tscn");
	private PackedScene WallCollider = (PackedScene)GD.Load("res://assets/biomes/WallCollider.tscn");
	private PackedScene TileCollider = (PackedScene)GD.Load("res://assets/biomes/TileCollider.tscn");
	private int selectedBiome = 4; //forest = 0, grassland = 1, desert = 2, tundra = 3, water = 4
	private bool mouseOnGUI = false;
	private bool isWorldBuilding = true;
	private Node TileSelectInst;
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
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

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

	private void AddTile(PackedScene tileType, Vector3 position){
		Node newTileInst = tileType.Instance();
		((Spatial)newTileInst).Translation = position;
		AddChild(newTileInst);
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
}



