using Godot;
using System;

public class SpeciesHolder : Spatial
{
	private int numOfSpecies = 0;
	private float CurrentWaitingTime = 0.0f;
	private const float WaitTime = 10f;
	private float TimeMultiplier = 1f;
	private bool SimulationStarted = false;

	private Godot.Collections.Array GlobalTimeArray = (Godot.Collections.Array) new Godot.Collections.Array();

	private PackedScene Species = (PackedScene)GD.Load("res://assets/Species.tscn");

	public override void _Ready()
	{
		SimulationStarted = Global.IsLoaded;
	}

	

	public void AddSpecies(String speciesName, int popSize, Color color, Godot.Collections.Array initialValues, float geneticVariation, int diet){
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

	public override void _Process(float delta){
		if(SimulationStarted){
			CurrentWaitingTime += TimeMultiplier * delta;
			if (CurrentWaitingTime >= WaitTime){
				UpdateChartData();
				CurrentWaitingTime = 0.0f;
			}
		}
	}

	private void _on_StartSimulation_pressed()
	{
		GlobalTimeArray.Add(0);
		SimulationStarted = true;
	}

	private void UpdateChartData()
	{
		GlobalTimeArray.Add(0);
		GetTree().CallGroup("Species", "CollectData");
		GetTree().CallGroup("GraphControl", "RefreshGraphs");
	}

	private void _on_SimulationRate_item_selected(int index)
	{
		switch (index){
			case 0:
				TimeMultiplier = 1f;
			break;
			case 1:
				TimeMultiplier = 2f;
			break;
			case 2:
				TimeMultiplier =4f;
			break;
		}
		GetTree().CallGroup("Species", "UpdateTimeMultiplier", TimeMultiplier);
	}

	public float GetTImeMultiplier(){
		return TimeMultiplier;
	}
	

}





