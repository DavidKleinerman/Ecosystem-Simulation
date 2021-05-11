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
		if(Global.IsLoaded){
			for (int i = 0; i < Global.LoadedGlobalTime.Count; i++)
				GlobalTimeArray.Add((float)Global.LoadedGlobalTime[i]);
			CurrentWaitingTime = Global.LoadedGlobalWaitingTime;
			GD.Print("the array: " + GlobalTimeArray);
			GD.Print(CurrentWaitingTime);
			foreach (Godot.Collections.Dictionary s in Global.LoadedSpecies){
				String speciesName = (String)s["SpeciesName"];
				Color color = new Color((float)s["SpeciesColorR"], (float)s["SpeciesColorG"], (float)s["SpeciesColorB"]);
				int diet = (int)((float)s["SpeciesDiet"]);
				Node newSpeciesInst = Species.Instance();
				((Species)newSpeciesInst).InitSpecies(speciesName, color, GlobalTimeArray, diet, true, s);
				AddChild(newSpeciesInst);
				((Species)newSpeciesInst).LoadCreatures((Godot.Collections.Array)s["Creatures"]);
				GetParent().GetNode<ItemList2>("DisplayChartsMenu/ItemList2").AddNewSpecies(speciesName);
			}
		}

	}

	public Godot.Collections.Array Save(){
		Godot.Collections.Array existingSpecies = GetTree().GetNodesInGroup("Species");
		Godot.Collections.Array savedSpecies = new Godot.Collections.Array();
		foreach (Species s in existingSpecies){
			savedSpecies.Add(s.Save());
		}
		return savedSpecies;
	}

	public void AddSpecies(String speciesName, int popSize, Color color, Godot.Collections.Array initialValues, float geneticVariation, int diet){
		Node newSpeciesInst = Species.Instance();
		AddChild(newSpeciesInst);
		((Species)newSpeciesInst).InitSpecies(speciesName, color, GlobalTimeArray, diet, false, null);
		((Species)newSpeciesInst).AddNewCreatures(popSize, initialValues, geneticVariation);
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
		GetParent().GetNode<BiomeGrid>("BiomeGrid").UpdatePlantBiomass();
		GetParent().GetNode<MultiMeshMeat>("MultiMeshMeat").UpdateMeatBiomass();
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
				TimeMultiplier = 4f;
			break;
		}
		GetTree().CallGroup("Species", "UpdateTimeMultiplier", TimeMultiplier);
	}

	public float GetTimeMultiplier(){
		return TimeMultiplier;
	}

	public Godot.Collections.Array GetGlobalTimeArray(){
		return GlobalTimeArray;
	}
	
	public float GetCurrentWaitingTime(){
		return CurrentWaitingTime;
	}

}





