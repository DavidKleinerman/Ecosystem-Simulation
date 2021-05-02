using Godot;
using System;

public class ItemList : Godot.ItemList
{
	public override void _Ready()
	{
	   Visible = !Global.IsLoaded;
	}
	
	private void _on_StartSimulation_pressed()
	{
		this.Visible = false;
	}	
}



