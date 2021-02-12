using Godot;
using System;

public class ThirstResistanceGradient : Line2D
{
	public override void _Ready()
	{
		this.Visible = false;
	}
	
	private void _on_ThirstResistanceCheckButton_toggled(bool button_pressed)
	{
		if(button_pressed){
			this.Visible = true;
		}
		if(!button_pressed){
			this.Visible = false;
		}
		GetTree().CallGroup("GraphControl", "RefreshGraphs");
	}
}
