using Godot;
using System;

public class AddNewSpecies : Button
{

	public override void _Ready()
	{
		this.Visible = Global.IsLoaded;
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
	private void _on_DisplayCharts_pressed()
	{
		this.Visible = false;
	}
	private void _on_CloseChartMenu_pressed()
	{
		this.Visible = true;
	}
	private void _on_Pause_pressed()
	{
		this.Visible = false;
	}
	private void _on_Resume_pressed()
	{
		this.Visible = true;
	}
}




