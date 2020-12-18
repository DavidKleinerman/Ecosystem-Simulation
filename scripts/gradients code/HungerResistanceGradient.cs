using Godot;
using System;

public class HungerResistanceGradient : Line2D
{
	public override void _Ready()
	{
		this.Visible = false;
	}
	
	private void _on_HungerResistanceCheckButton_toggled(bool button_pressed)
	{
		if(button_pressed){
			this.Visible = true;
		}
		if(!button_pressed){
			this.Visible = false;
		}
	}
}
