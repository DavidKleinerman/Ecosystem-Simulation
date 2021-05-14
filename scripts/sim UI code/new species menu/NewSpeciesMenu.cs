using Godot;
using System;

public class NewSpeciesMenu : Control
{
	public override void _Ready()
	{
		this.Visible = false;
	}
	private void _on_AddNewSpecies_pressed()
	{
		this.Visible = true;
	}

	private void _on_CloseNewSpecies_pressed()
	{
		this.Visible = false;
	}

}
