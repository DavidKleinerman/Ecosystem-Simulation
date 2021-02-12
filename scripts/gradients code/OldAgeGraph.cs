using Godot;
using System;

public class OldAgeGraph : Line2D
{

	public override void _Ready()
	{
		this.Visible = false;
	}

	private void _on_DeathsFromOldAgeCheckButton_toggled(bool button_pressed)
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



