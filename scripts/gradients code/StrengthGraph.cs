using Godot;
using System;

public class StrengthGraph : Line2D
{
	public override void _Ready()
	{
		this.Visible = false;
	}
	private void _on_StrengthCheckButton_toggled(bool button_pressed)
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



