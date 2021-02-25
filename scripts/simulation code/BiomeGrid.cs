using Godot;
using System;

public class BiomeGrid : GridMap
{
	private PackedScene TileSelector = (PackedScene)GD.Load("res://assets/TileSelector.tscn");
	private PackedScene WallCollider = (PackedScene)GD.Load("res://assets/biomes/WallCollider.tscn");
	private PackedScene TileCollider = (PackedScene)GD.Load("res://assets/biomes/TileCollider.tscn");
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
		//TileSelectInst = TileSelector.Instance();
		//AddChild(TileSelectInst);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
