using Godot;
using System;

public class NewSimMenu : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	private int WorldSize = Global.worldSize;
	private int BiomeType = Global.biomeType;
	private float BiomeGrowthRate = Global.biomeGrowthRate;
	public override void _Ready()
	{
		this.Visible = true;
		if(this.WorldSize == 32){
			GetNode<Godot.ItemList>("WorldSizePicker").Select(2);
		}
		else if(this.WorldSize == 24){
			GetNode<Godot.ItemList>("WorldSizePicker").Select(1);
		}
		else if(this.WorldSize == 16){
			GetNode<Godot.ItemList>("WorldSizePicker").Select(0);
		}
		GetNode<Godot.ItemList>("BiomeType").Select(BiomeType);
		GetNode<VSlider>("VSlider").Value = BiomeGrowthRate;
		GetViewport().Size = Global.Resolution;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
