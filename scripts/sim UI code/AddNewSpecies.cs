using Godot;
using System;

public class AddNewSpecies : Button
{

	public override void _Ready()
	{
		this.Visible = false;
	}

	private void _on_StartSimulation_pressed()
	{
		this.Visible = true;
	}

	private void _on_AddNewSpecies_pressed()
	{
		this.Visible = false;
	}
	private void _on_CloseNewSpecies_pressed()
	{
		this.Visible = true;
	}

}








