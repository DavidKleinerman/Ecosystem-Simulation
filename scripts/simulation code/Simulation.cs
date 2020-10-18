using Godot;
using System;

public class Simulation : Spatial
{
	private PackedScene WaterTile = (PackedScene)GD.Load("res://assets/biomes/WaterTile.tscn");
	private PackedScene ForestTile = (PackedScene)GD.Load("res://assets/biomes/ForestTile.tscn");
	private PackedScene DesertTile = (PackedScene)GD.Load("res://assets/biomes/DesertTile.tscn");
	private PackedScene GrasslandTile = (PackedScene)GD.Load("res://assets/biomes/GrasslandTile.tscn");
	private PackedScene TundraTile = (PackedScene)GD.Load("res://assets/biomes/TundraTile.tscn");

	private int selectedBiome = -1; //forest = 0, grassland = 1, desert = 2, tundra = 3, water = 5, none = -1
	private bool isWorldBuilding = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Vector3 position = (Vector3) new Vector3(0,0,0);
		position.x = -64;
		position.z = -64;
		for(int i = 0; i < 32; i++){
			for(int j = 0; j < 32; j++){
				Node newTileInst = WaterTile.Instance();
				((Spatial)newTileInst).Translation = position;
				AddChild(newTileInst);
				position.z += 4;
			}
			position.x += 4;
			position.z = -64;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		

	}

	public override void _PhysicsProcess(float delta)
	{
		if (isWorldBuilding && Input.IsActionPressed("ui_select"))
			ReplaceBiome();
	}

	private void _on_ItemList_item_selected(int index)
	{
		selectedBiome = index;
		GD.Print(index, "\n");
	}

	private void ReplaceBiome(){
		Node selectedTile = GetObjectUnderMouse();
		GD.Print(selectedTile.Name);
		if (selectedTile.Name == "StaticBody"){
			Vector3 selectedPos = ((Spatial)selectedTile).ToGlobal(((Spatial)selectedTile).Translation);
			GD.Print(selectedPos);
			foreach (Node n in GetChildren()){
				if (n.Name != "Control" && n.Name != "SpeciesHolder"){
					Vector3 position = ((Spatial)n).Translation;
					if (position.x == selectedPos.x && position.y == selectedPos.y && position.z == selectedPos.z){
						GD.Print("Found it!\n");
						n.QueueFree();
						break;
					}
				}
			}
			//if (selectedBiome == 0)
		}

	}

	private Node GetObjectUnderMouse(){
		Vector2 mousePos = (Vector2) new Vector2(0, 0);
		mousePos = GetViewport().GetMousePosition();
		Vector3 rayFrom = ToGlobal(((Camera)GetNode("CameraHolder/Camera")).ProjectRayOrigin(mousePos));
		Vector3 rayTo = rayFrom + ToGlobal(((Camera)GetNode("CameraHolder/Camera")).ProjectRayNormal(mousePos)) * 20000;
		PhysicsDirectSpaceState spaceState = GetWorld().DirectSpaceState;
		var hit = spaceState.IntersectRay(rayFrom, rayTo);
		Node selection = (Node) new Node();
		if (hit.Contains("collider"))
			selection = (Node)hit["collider"];
		return selection;
	}
}



