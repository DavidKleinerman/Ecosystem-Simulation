using Godot;
using System;

public class ItemList2 : ItemList
{
	public Godot.Collections.Array SpeciesItems = (Godot.Collections.Array) new Godot.Collections.Array();
	
	public override void _Ready()
	{
		this.Visible = false;    
	}
	private void _on_DisplayCharts_pressed()
	{
   		this.Visible = true;
	}
	public void AddNewSpecies(String species){
		AddItem(species);
		SpeciesItems.Add(species);
		
	}
	public String GetItem(int index){
		GD.Print(SpeciesItems[index], "\n");
		return (String)SpeciesItems[index];
	}
}
