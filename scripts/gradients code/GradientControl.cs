using Godot;
using System;

public class GradientControl : Control
{
	//pop size
	private Line2D linePopSize;
	//genetic traits
   	private Line2D lineSpeed;
	private Line2D linePerception;
	private Line2D lineMatingCycle;
	private Line2D lineHungerResistance;
	private Line2D lineThirstResistance;
	private Line2D lineGestation;
	private Line2D lineLongevity;
	private Line2D lineLitterSize;
	private Line2D lineIntelligence;
	private Line2D lineMemory;
	private Line2D lineStrength;
	private Line2D lineHeatResistance;
	private Line2D lineColdResistance;
	private Line2D lineStamina;
	//causes of death
	private Line2D lineStarvation;
	private Line2D lineDehydration;
	private Line2D lineOldAge;
	private Line2D lineBeingHunted;
	private Line2D lineHeatStroke;
	private Line2D lineFreezing;
	//*****************
	private float MaxYvalue = 100;
	//pop size arrays
	public Godot.Collections.Array PopulationSizeArray;
	//genetic trait arrays
	public Godot.Collections.Array SpeedArray;
	public Godot.Collections.Array PerceptionArray; 
	public Godot.Collections.Array MatingCycleArray;
	public Godot.Collections.Array GestationArray;
	public Godot.Collections.Array HungerResistanceArray;
	public Godot.Collections.Array ThirstResistanceArray;
	public Godot.Collections.Array LongevityArray;
	public Godot.Collections.Array LitterSizeArray;
	public Godot.Collections.Array IntelligenceArray;
	public Godot.Collections.Array MemoryArray;
	public Godot.Collections.Array StrengthArray;
	public Godot.Collections.Array HeatResistanceArray;
	public Godot.Collections.Array ColdResistanceArray;
	public Godot.Collections.Array StaminaArray;
	//causes of death arrays
	public Godot.Collections.Array StarvationArray;
	public Godot.Collections.Array DehydrationArray;
	public Godot.Collections.Array OldAgeArray;
	public Godot.Collections.Array BeingHuntedArray;
	public Godot.Collections.Array HeatStrokeArray;
	public Godot.Collections.Array FreezingArray;
	//**********
	private bool OpenedUp = false;
	private int selectedSpecies;
	public String SelectedSpecies;
	public override void _Ready()
	{
		//pop size
		linePopSize = GetNode<Line2D>("PopulationSizeGraph");
		//genetic tratis
		lineSpeed = GetNode<Line2D>("SpeedGradient");
		linePerception = GetNode<Line2D>("PerceptionGradient");
		lineGestation = GetNode<Line2D>("GestationGradient");
		lineMatingCycle = GetNode<Line2D>("MatingCycleGradient");
		lineThirstResistance = GetNode<Line2D>("ThirstResistanceGradient");
		lineLongevity = GetNode<Line2D>("LongevityGradient");
		lineLitterSize = GetNode<Line2D>("LitterSizeGradient");
		lineHungerResistance = GetNode<Line2D>("HungerResistanceGradient");
		lineIntelligence = GetNode<Line2D>("IntelligenceGraph");
		lineMemory = GetNode<Line2D>("MemoryGraph");
		lineStrength = GetNode<Line2D>("StrengthGraph");
		lineHeatResistance = GetNode<Line2D>("HeatResistanceGraph");
		lineColdResistance = GetNode<Line2D>("ColdResistanceGraph");
		lineStamina = GetNode<Line2D>("StaminaGraph");
		//causes of death
		lineStarvation = GetNode<Line2D>("StarvationGraph");
		lineDehydration = GetNode<Line2D>("DehydrationGraph");
		lineOldAge = GetNode<Line2D>("OldAgeGraph");
		lineBeingHunted = GetNode<Line2D>("BeingHuntedGraph");
		lineHeatStroke = GetNode<Line2D>("HeatStrokeGraph");
		lineFreezing = GetNode<Line2D>("FreezingGraph");
	}
	public void RefreshGraphs(){
		
		//DataCollector sp =  GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies);
		//SpeedArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").RandomArrData();
		if(SelectedSpecies != null && OpenedUp){
			DataCollector speciesData = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies);
			int speciesCreationTime = speciesData.GetSpeciesCreationTime();
			PopulationSizeArray = speciesData.GetPopulationSizeData();
			SpeedArray = speciesData.GetSpeedData();
			PerceptionArray = speciesData.GetPerceptionData();
			GestationArray = speciesData.GetGestationData();
			MatingCycleArray = speciesData.GetMatingCycleData();
			HungerResistanceArray = speciesData.GetHungerResistanceData();
			ThirstResistanceArray = speciesData.GetThirstResistanceData();
			LongevityArray = speciesData.GetLongevityData();
			LitterSizeArray = speciesData.GetLitterSizeData();
			IntelligenceArray = speciesData.GetIntelligenceData();
			MemoryArray = speciesData.GetMemoryData();
			StrengthArray = speciesData.GetStrengthData();
			HeatResistanceArray = speciesData.GetHeatResistanceData();
			ColdResistanceArray = speciesData.GetColdResistancehData();
			StaminaArray = speciesData.GetStaminaData();

			StarvationArray = speciesData.GetStarvationData();
			DehydrationArray = speciesData.GetDehydrationData();
			OldAgeArray = speciesData.GetOldAgeData();
			BeingHuntedArray = speciesData.GetBeingHuntedData();
			HeatStrokeArray = speciesData.GetHeatStrokeData();
			FreezingArray = speciesData.GetFreezingData();
			CalculateMax();
			GetNode<Label>("MidLabel").Text = "" + MaxYvalue/2;
			GetNode<Label>("MaxLabel").Text = "" + MaxYvalue;
			DrawGraph(PopulationSizeArray, linePopSize, speciesCreationTime);
			DrawGraph(SpeedArray, lineSpeed, speciesCreationTime);
			DrawGraph(PerceptionArray, linePerception, speciesCreationTime);
			DrawGraph(GestationArray, lineGestation, speciesCreationTime);
			DrawGraph(MatingCycleArray, lineMatingCycle, speciesCreationTime);
			DrawGraph(HungerResistanceArray, lineHungerResistance, speciesCreationTime);
			DrawGraph(ThirstResistanceArray, lineThirstResistance, speciesCreationTime);
			DrawGraph(LongevityArray, lineLongevity, speciesCreationTime);
			DrawGraph(LitterSizeArray, lineLitterSize, speciesCreationTime);
			DrawGraph(IntelligenceArray, lineIntelligence, speciesCreationTime);
			DrawGraph(MemoryArray, lineMemory, speciesCreationTime);
			DrawGraph(StrengthArray, lineStrength, speciesCreationTime);
			DrawGraph(HeatResistanceArray, lineHeatResistance, speciesCreationTime);
			DrawGraph(ColdResistanceArray, lineColdResistance, speciesCreationTime);
			DrawGraph(StaminaArray, lineStamina, speciesCreationTime);

			DrawGraph(StarvationArray, lineStarvation, speciesCreationTime);
			DrawGraph(DehydrationArray, lineDehydration, speciesCreationTime);
			DrawGraph(OldAgeArray, lineOldAge, speciesCreationTime);
			DrawGraph(BeingHuntedArray, lineBeingHunted, speciesCreationTime);
			DrawGraph(HeatStrokeArray,lineHeatStroke, speciesCreationTime);
			DrawGraph(FreezingArray, lineFreezing, speciesCreationTime);
		}
	}

	private void CalculateMax(){
		Godot.Collections.Array arrayOfMaxes = (Godot.Collections.Array) new Godot.Collections.Array();
		if (IsAnyGeneticTraitVisible())
			arrayOfMaxes.Add(100f);
		if(linePopSize.Visible)
			arrayOfMaxes.Add(FindMaxInArray(PopulationSizeArray));
		if(lineStarvation.Visible)
			arrayOfMaxes.Add(FindMaxInArray(StarvationArray));
		if(lineDehydration.Visible)
			arrayOfMaxes.Add(FindMaxInArray(DehydrationArray));
		if(lineOldAge.Visible)
			arrayOfMaxes.Add(FindMaxInArray(OldAgeArray));
		if(lineBeingHunted.Visible)
			arrayOfMaxes.Add(FindMaxInArray(BeingHuntedArray));
		if(lineHeatStroke.Visible)
			arrayOfMaxes.Add(FindMaxInArray(HeatStrokeArray));
		if(lineFreezing.Visible)
			arrayOfMaxes.Add(FindMaxInArray(FreezingArray));
		if(arrayOfMaxes.Count > 0)
			MaxYvalue = FindMaxInArray(arrayOfMaxes);
		// if (linePopSize.Visible){
		// 	float max = 0;
		// 	for(int i=0; i < PopulationSizeArray.Count; i++){
		// 		if (i == 0)
		// 			max = (float)PopulationSizeArray[i];
		// 		else if ((float)PopulationSizeArray[i] > max)
		// 			max = (float)PopulationSizeArray[i];
		// 	}
		// 	if (max > 100)
		// 		MaxYvalue = max;
		// 	else MaxYvalue = 100;
		// } else MaxYvalue = 100;

	}

	private float FindMaxInArray(Godot.Collections.Array array){
		float max = 0;
		for(int i=0; i < array.Count; i++){
			if (i == 0)
				max = (float)array[i];
			else if ((float)array[i] > max)
				max = (float)array[i];
		}
		return max;
	}

	private bool IsAnyGeneticTraitVisible(){
		return (lineSpeed.Visible || linePerception.Visible ||
				lineMatingCycle.Visible || lineHungerResistance.Visible ||
				lineThirstResistance.Visible || lineGestation.Visible ||
				lineLongevity.Visible || lineLitterSize.Visible ||
				lineIntelligence.Visible || lineMemory.Visible ||
				lineStrength.Visible || lineHeatResistance.Visible ||
				lineColdResistance.Visible || lineStamina.Visible);
	}

	private void DrawGraph(Godot.Collections.Array dataArray, Line2D line, int speciesCreationTime){
		if(line.Visible){
			line.ClearPoints();
			for(int i=0; i < dataArray.Count; i++){
				Vector2 NewPoint;
				if ( i == speciesCreationTime + 1){
					Vector2 creationPoint = (Vector2) new Vector2((i+1)*1350/dataArray.Count ,520);
					line.AddPoint(creationPoint);
				}
				if(i == 0){
					if(MaxYvalue != 0)
						NewPoint = (Vector2) new Vector2(10 ,520-(float)dataArray[i]*520f/MaxYvalue);
					else
						NewPoint = (Vector2) new Vector2(10 ,520);
				}
				else{
					if(MaxYvalue != 0)
						NewPoint = (Vector2) new Vector2((i+1)*1350/dataArray.Count ,520-(float)dataArray[i]*520f/MaxYvalue);
					else
						NewPoint = (Vector2) new Vector2((i+1)*1350/dataArray.Count ,520);
				}
				line.AddPoint(NewPoint);
			}
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


