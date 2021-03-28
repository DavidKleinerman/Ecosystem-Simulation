using Godot;
using System;

public class Pause : Button
{
	public override void _Ready()
	{
		this.Visible = false;
	}
	private void _on_StartSimulation_pressed()
	{
		this.Visible = true;
	}
	private void _on_Pause_pressed()
	{
		this.Visible = false;
		GetTree().Paused = true;
	}
	private void _on_Resume_pressed()
	{
		this.Visible = true;
		GetTree().Paused = false;
	}
	private void _on_AddNewSpecies_pressed()
	{
		this.Visible = false;
	}
	private void _on_DisplayCharts_pressed()
	{
		this.Visible = false;
	}
	private void _on_CloseNewSpecies_pressed()
	{
		this.Visible = true;
	}
	private void _on_CloseChartMenu_pressed()
	{
		this.Visible = true;
	}
}







