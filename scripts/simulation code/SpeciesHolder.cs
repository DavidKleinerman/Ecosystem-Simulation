using Godot;
using System;

public class SpeciesHolder : Spatial
{
	private int numOfSpecies = 0;

	private Timer GlobalTime;

	private Godot.Collections.Array GlobalTimeArray = (Godot.Collections.Array) new Godot.Collections.Array();

	private PackedScene Species = (PackedScene)GD.Load("res://assets/Species.tscn");

	public override void _Ready(){
		GlobalTime = GetNode<Timer>("GlobalTimeTicks");
	}
	public void AddSpecies(String speciesName, int popSize, Color color, Godot.Collections.Array initialValues, float geneticVariation, int diet){
		GD.Print("called add species in holder");
		Node newSpeciesInst = Species.Instance();
		AddChild(newSpeciesInst);
		((Species)newSpeciesInst).InitSpecies(speciesName, GlobalTimeArray, diet);
		((Species)newSpeciesInst).AddNewCreatures(popSize, color, initialValues, geneticVariation);
		GetParent().GetNode<ItemList2>("DisplayChartsMenu/ItemList2").AddNewSpecies(speciesName);
	}

	public DataCollector GetDataOfSpecies(String species){
		Godot.Collections.Array existingSpecies = GetTree().GetNodesInGroup("Species");
		foreach (Species s in existingSpecies){
			if (s.GetSpeciesName() == species)
				return s.GetDataCollector();
		}
		return null;
	}

	public void AddDead(Vector3 position){
		PackedScene Meat = (PackedScene)GD.Load("res://assets/Meat.tscn");
		Node meatInst = Meat.Instance();
		((Spatial)meatInst).Translation = position;
		AddChild(meatInst);
	}

	private void _on_StartSimulation_pressed()
	{
		GlobalTimeArray.Add(0);
		GlobalTime.WaitTime = 10;
		GlobalTime.Start();
	}

	private void _on_GlobalTimeTicks_timeout()
	{
		GlobalTimeArray.Add(0);
		GetTree().CallGroup("Species", "CollectData");
		GetTree().CallGroup("GraphControl", "RefreshGraphs");
	}
	

}


