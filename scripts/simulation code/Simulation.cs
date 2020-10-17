using Godot;
using System;

public class Simulation : Spatial
{
	private PackedScene WaterTile = (PackedScene)GD.Load("res://assets/biomes/WaterTile.tscn");
	private PackedScene ForestTile = (PackedScene)GD.Load("res://assets/biomes/ForestTile.tscn");
	private PackedScene DesertTile = (PackedScene)GD.Load("res://assets/biomes/DesertTile.tscn");
	private PackedScene GrasslandTile = (PackedScene)GD.Load("res://assets/biomes/GrasslandTile.tscn");
	private PackedScene TundraTile = (PackedScene)GD.Load("res://assets/biomes/TundraTile.tscn");

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

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
