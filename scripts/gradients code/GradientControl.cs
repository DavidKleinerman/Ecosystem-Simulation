using Godot;
using System;

public class GradientControl : Control
{
   	private Line2D lineSpeed;
	private Line2D lineStamina;
	private Line2D linePerception;
	private Line2D lineMatingCycle;
	private Line2D lineHungerResistance;
	private Line2D lineThirstResistance;
	private Line2D lineGestation;
	private Line2D lineLongevity;
	private Line2D lineLitterSize;
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
		//this.Visible = false;
		
	}
	public override void _Process(float delta){
		
		//DataCollector sp =  GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies);
		//SpeedArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").RandomArrData();
		if(SelectedSpecies != null && OpenedUp){
			SpeedArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetSpeedData();
			PerceptionArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetPerceptionData();
			GestationArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetGestationData();
			MatingCycleArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetMatingCycleData();
			HungerResistanceArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetHungerResistanceData();
			ThirstResistanceArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetThirstResistanceData();
			LongevityArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetLongevityData();
			LitterSizeArray = GetParent().GetParent().GetNode<SpeciesHolder>("SpeciesHolder").GetDataOfSpecies(SelectedSpecies).GetLitterSizeData();
			RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
			Godot.Collections.Array arr = (Godot.Collections.Array) new Godot.Collections.Array();
			for (int i = 0; i < 100; i++){
				rng.Randomize();
				arr.Add(rng.Randf());
			}
			lineStamina = GetNode<Line2D>("StaminaGradient");
			lineStamina.ClearPoints();
			for(int i=0; i < 10; i++){
				Vector2 NewPoint = (Vector2) new Vector2(i*98 ,370 +(float)arr[i]*30);
				lineStamina.AddPoint(NewPoint);
			}
			//AddChild(line);
			lineSpeed = GetNode<Line2D>("SpeedGradient");
			lineSpeed.ClearPoints();
			for(int i=0; i < SpeedArray.Count; i++){
				//float Value = (float)(sp.GetSpeedData()[i]);
				//GD.Print("The current speed in index:\n" +i + "\t" +SpeedArray[i]);
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)SpeedArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/SpeedArray.Count ,415-(float)SpeedArray[i]*4.15f);
				}
				lineSpeed.AddPoint(NewPoint);
			}
			linePerception = GetNode<Line2D>("PerceptionGradient");
			linePerception.ClearPoints();
			for(int i=0; i < PerceptionArray.Count; i++){
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)PerceptionArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/PerceptionArray.Count ,415-(float)PerceptionArray[i]*4.15f);
				}
				linePerception.AddPoint(NewPoint);
			}
			lineGestation = GetNode<Line2D>("GestationGradient");
			lineGestation.ClearPoints();
			for(int i=0; i< GestationArray.Count; i++){
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)GestationArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/GestationArray.Count ,415-(float)GestationArray[i]*4.15f);
				}
				lineGestation.AddPoint(NewPoint);
			}
			lineMatingCycle = GetNode<Line2D>("MatingCycleGradient");
			lineMatingCycle.ClearPoints();
			for(int i=0; i < MatingCycleArray.Count; i++){
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)MatingCycleArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/MatingCycleArray.Count ,415-(float)MatingCycleArray[i]*4.15f);
				}
				lineMatingCycle.AddPoint(NewPoint);
			}
			lineThirstResistance = GetNode<Line2D>("ThirstResistanceGradient");
			lineThirstResistance.ClearPoints();
			for(int i=0; i < ThirstResistanceArray.Count; i++){
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)ThirstResistanceArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/ThirstResistanceArray.Count ,415-(float)ThirstResistanceArray[i]*4.15f);
				}
				lineThirstResistance.AddPoint(NewPoint);
			}
			lineLongevity = GetNode<Line2D>("LongevityGradient");
			lineLongevity.ClearPoints();
			for(int i=0; i< LongevityArray.Count; i++){
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)LongevityArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/LongevityArray.Count ,415-(float)LongevityArray[i]*4.15f);
				}
				lineLongevity.AddPoint(NewPoint);
			}
			lineLitterSize = GetNode<Line2D>("LitterSizeGradient");
			lineLitterSize.ClearPoints();
			for(int i=0; i< LitterSizeArray.Count; i++){
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)LitterSizeArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/LitterSizeArray.Count ,415-(float)LitterSizeArray[i]*4.15f);
				}
				lineLitterSize.AddPoint(NewPoint);
			}
			lineHungerResistance = GetNode<Line2D>("HungerResistanceGradient");
			lineHungerResistance.ClearPoints();
			for(int i=0; i < HungerResistanceArray.Count; i++){
				Vector2 NewPoint;
				if(i == 0){
					NewPoint = (Vector2) new Vector2(10 ,415-(float)HungerResistanceArray[i]*4.15f);
				}
				else{
					NewPoint = (Vector2) new Vector2((i+1)*880/HungerResistanceArray.Count ,415-(float)HungerResistanceArray[i]*4.15f);
				}
				lineHungerResistance.AddPoint(NewPoint);
			}
		}
	}
	private void _on_ItemList2_item_selected(int index)
	{
		selectedSpecies = index;
		GD.Print(index, "\n");
		SelectedSpecies = GetParent().GetNode<ItemList2>("ItemList2").GetItem(index);
		
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


