using Godot;
using System;

public class BiomeGrid : GridMap
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
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
		//TileSelectInst = TileSelector.Instance();
		//AddChild(TileSelectInst);
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
