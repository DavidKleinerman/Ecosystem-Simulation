using Godot;
using System;

public class MatingCycleGradient : Line2D
{
	public override void _Ready()
	{
		this.Visible = false;
	}
	
	private void _on_MatingCycleCheckButton_toggled(bool button_pressed)
	{
		if(button_pressed){
			this.Visible = true;
		}
		if(!button_pressed){
			this.Visible = false;
		}
	}
}
