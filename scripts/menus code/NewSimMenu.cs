using Godot;
using System;

public class NewSimMenu : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = true;
		GetNode<Godot.ItemList>("WorldSizePicker").Select(2);
		GetNode<Godot.ItemList>("BiomeType").Select(0);
		GetViewport().Size = Global.Resolution;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
