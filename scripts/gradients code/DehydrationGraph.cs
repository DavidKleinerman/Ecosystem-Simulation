using Godot;
using System;

public class DehydrationGraph : Line2D
{
	
	public override void _Ready()
	{
		this.Visible = false;
	}

	private void _on_DeathsFromDehydrationCheckButton_toggled(bool button_pressed)
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



