using Godot;
using System;

public class HeatResistanceGraph : Line2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Visible = false;
	}

	private void _on_HeatResistanceCheckButton_toggled(bool button_pressed)
	{
		if(button_pressed){
			this.Visible = true;
		}
		else {
			this.Visible = false;
		}
		GetTree().CallGroup("GraphControl", "RefreshGraphs");
	}
}



