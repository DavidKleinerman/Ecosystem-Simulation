using Godot;
using System;

public class GradientControl : Control
{
	private Line2D linePopSize;
   	private Line2D lineSpeed;
	private Line2D lineStamina;
	private Line2D linePerception;
	private Line2D lineMatingCycle;
	private Line2D lineHungerResistance;
	private Line2D lineThirstResistance;
	private Line2D lineGestation;
	private Line2D lineLongevity;
	private Line2D lineLitterSize;
	private float MaxYvalue;
	public Godot.Collections.Array PopulationSizeArray;
	public Godot.Collections.Array SpeedArray; //= (Godot.Collections.Array) new Godot.Collections.Array();
	public Godot.Collections.Array PerceptionArray; // = (Godot.Collections.Array) new Godot.Collections.Array();
	public Godot.Collections.Array MatingCycleArray;// = (Godot.Collections.Array) new Godot.Collections.Array();
	public Godot.Collections.Array GestationArray;// = (Godot.Collections.Array) new Godot.Collections.Array();
	public Godot.Collections.Array HungerResistanceArray;// = (Godot.Collections.Array) new Godot.Collections.Array();
	public Godot.Collections.Array ThirstResistanceArray;// = (Godot.Collections.Array) new Godot.Collections.Array();
	public Godot.Collections.Array StaminaArray;// = (Godot.Collections.Array) new Godot.Collections.Array();
	public Godot.Collections.Array LongevityArray;
	public Godot.Collections.Array LitterSizeArray;
	private bool OpenedUp = false;
	private int selectedSpecies;
	public String SelectedSpecies;
	public override void _Ready()
	{
		linePopSize = GetNode<Line2D>("PopulationSizeGraph");
		lineSpeed = GetNode<Line2D>("SpeedGradient");
		linePerception = GetNode<Line2D>("PerceptionGradient");
		lineGestation = GetNode<Line2D>("GestationGradient");
		lineMatingCycle = GetNode<Line2D>("MatingCycleGradient");
		lineThirstResistance = GetNode<Line2D>("ThirstResistanceGradient");
		lineLongevity = GetNode<Line2D>("LongevityGradient");
		lineLitterSize = GetNode<Line2D>("LitterSizeGradient");
		lineHungerResistance = GetNode<Line2D>("HungerResistanceGradient");
	}
	public void RefreshGraphs(){
		
		//DataCollector sp =  GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies);
		//SpeedArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").RandomArrData();
		if(SelectedSpecies != null && OpenedUp){
			CalculateMax();
			GetNode<Label>("MidLabel").Text = "" + MaxYvalue/2;
			GetNode<Label>("MaxLabel").Text = "" + MaxYvalue;
			int speciesCreationTime = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetSpeciesCreationTime();
			PopulationSizeArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetPopulationSizeData();
			SpeedArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetSpeedData();
			PerceptionArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetPerceptionData();
			GestationArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetGestationData();
			MatingCycleArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetMatingCycleData();
			HungerResistanceArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetHungerResistanceData();
			ThirstResistanceArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetThirstResistanceData();
			LongevityArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetLongevityData();
			LitterSizeArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetLitterSizeData();

			DrawGraph(PopulationSizeArray, linePopSize, speciesCreationTime);
			DrawGraph(SpeedArray, lineSpeed, speciesCreationTime);
			DrawGraph(PerceptionArray, linePerception, speciesCreationTime);
			DrawGraph(GestationArray, lineGestation, speciesCreationTime);
			DrawGraph(MatingCycleArray, lineMatingCycle, speciesCreationTime);
			DrawGraph(HungerResistanceArray, lineHungerResistance, speciesCreationTime);
			DrawGraph(ThirstResistanceArray, lineThirstResistance, speciesCreationTime);
			DrawGraph(LongevityArray, lineLongevity, speciesCreationTime);
			DrawGraph(LitterSizeArray, lineLitterSize, speciesCreationTime);
		}
	}

	private void CalculateMax(){
		if (linePopSize.Visible){
			float max = 0;
			for(int i=0; i < PopulationSizeArray.Count; i++){
				if (i == 0)
					max = (float)PopulationSizeArray[i];
				else if ((float)PopulationSizeArray[i] > max)
					max = (float)PopulationSizeArray[i];
			}
			if (max > 100)
				MaxYvalue = max;
			else MaxYvalue = 100;
		} else MaxYvalue = 100;

	}

	private void DrawGraph(Godot.Collections.Array dataArray, Line2D line, int speciesCreationTime){
		line.ClearPoints();
		for(int i=0; i < dataArray.Count; i++){
			Vector2 NewPoint;
			if ( i == speciesCreationTime + 1){
				Vector2 creationPoint = (Vector2) new Vector2((i+1)*880/dataArray.Count ,415);
				line.AddPoint(creationPoint);
			}
			if(i == 0){
				NewPoint = (Vector2) new Vector2(10 ,415-(float)dataArray[i]*415f/MaxYvalue);
			}
			else{
				NewPoint = (Vector2) new Vector2((i+1)*880/dataArray.Count ,415-(float)dataArray[i]*415f/MaxYvalue);
			}
			line.AddPoint(NewPoint);
		}
	}
	private void _on_ItemList2_item_selected(int index)
	{
		selectedSpecies = index;
		GD.Print(index, "\n");
		SelectedSpecies = GetParent().GetNode<ItemList2>("ItemList2").GetItem(index);
		RefreshGraphs();
	}

	private void _on_DisplayCharts_pressed()
	{
		OpenedUp = true;
	}


	private void _on_CloseChartMenu_pressed()
	{
		OpenedUp = false;
	}

}


