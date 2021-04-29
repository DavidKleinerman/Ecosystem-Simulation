using Godot;
using System;

public class SimulationRate : ItemList
{

	public override void _Ready()
	{
		Visible = false;
		GetParent().GetNode<Label>("Label").Visible = false;
	}

	private void _on_StartSimulation_pressed()
	{
		Visible = true;
		GetParent().GetNode<Label>("Label").Visible = true;
	}

}



