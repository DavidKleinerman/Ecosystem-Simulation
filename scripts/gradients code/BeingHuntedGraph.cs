using Godot;
using System;

public class BeingHuntedGraph : Line2D
{
	public override void _Ready()
	{
		this.Visible = false;
	}

	private void _on_DeathsFromBeingHuntedCheckButton_toggled(bool button_pressed)
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


