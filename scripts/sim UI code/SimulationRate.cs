using Godot;
using System;

public class SimulationRate : ItemList
{

	public override void _Ready()
	{
		Visible = Global.IsLoaded;
		GetParent().GetNode<Label>("Label").Visible = Global.IsLoaded;
	}

	private void _on_StartSimulation_pressed()
	{
		MakeVisible();
	}

	void MakeVisible(){
		Visible = true;
		GetParent().GetNode<Label>("Label").Visible = true;
	}

	void MakeInvisible(){
		Visible = false;
		GetParent().GetNode<Label>("Label").Visible = false;
	}

	private void _on_AddNewSpecies_pressed()
	{
		MakeInvisible();
	}

	private void _on_DisplayCharts_pressed()
	{
		MakeInvisible();
	}

	private void _on_Pause_pressed()
	{
		MakeInvisible();
	}
	
	private void _on_CloseNewSpecies_pressed()
	{
		MakeVisible();
	}
	
	private void _on_CloseChartMenu_pressed()
	{
		MakeVisible();
	}
	
	private void _on_Resume_pressed()
	{
		MakeVisible();
	}

}
