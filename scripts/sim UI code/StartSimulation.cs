using Godot;
using System;

public class StartSimulation : Button
{

	public override void _Ready()
	{
		this.Visible = !Global.IsLoaded;
	}
	private void _on_StartSimulation_pressed()
	{
		this.Visible = false;
	}

}
