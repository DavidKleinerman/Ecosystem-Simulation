using Godot;
using System;

public class DataCollector
{
	private int CurrentTimeTick = 0;
	private int SpeciesCreationTime;
	private Godot.Collections.Array PopulationSizeArray = (Godot.Collections.Array)new Godot.Collections.Array();
	//genetic traits
	private Godot.Collections.Array MaleFitness = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array SpeedArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array PerceptionArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array MatingCycleArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array HungerResistanceArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array ThirstResistanceArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array GestationArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array LitterSizeArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array LongevityArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array IntelligenceArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array MemoryArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array StrengthArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array HeatResistanceArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array ColdResistanceArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array StaminaArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array SleepCycleArray = (Godot.Collections.Array)new Godot.Collections.Array();
	//causes of death
	private Godot.Collections.Array StarvationArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array DehydrationArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array OldAgeArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array BeingHuntedArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array HeatStrokeArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private Godot.Collections.Array FreezingArray = (Godot.Collections.Array)new Godot.Collections.Array();
	private int CurrentStarvationAmount = 0;
	private int CurrentDehydrationAmount = 0;
	private int CurrentOldAgeAmount = 0;
	private int CurrentBeingHuntedAmount = 0;
	private int CurrentHeatStrokeAmount = 0;
	private int CurrentFreezingAmount = 0;

	public DataCollector(Godot.Collections.Array initArray)
	{
		for (int i = 0; i < initArray.Count; i++)
		{
			PopulationSizeArray.Add(0.0f);
			MaleFitness.Add(0.0f);
			SpeedArray.Add(0.0f);
			PerceptionArray.Add(0.0f);
			MatingCycleArray.Add(0.0f);
			HungerResistanceArray.Add(0.0f);
			ThirstResistanceArray.Add(0.0f);
			GestationArray.Add(0.0f);
			LongevityArray.Add(0.0f);
			LitterSizeArray.Add(0.0f);
			IntelligenceArray.Add(0.0f);
			MemoryArray.Add(0.0f);
			StrengthArray.Add(0.0f);
			HeatResistanceArray.Add(0.0f);
			ColdResistanceArray.Add(0.0f);
			StaminaArray.Add(0.0f);
			SleepCycleArray.Add(0.0f);

			StarvationArray.Add(0.0f);
			DehydrationArray.Add(0.0f);
			OldAgeArray.Add(0.0f);
			BeingHuntedArray.Add(0.0f);
			HeatStrokeArray.Add(0.0f);
			FreezingArray.Add(0.0f);
		}
		CurrentTimeTick = initArray.Count - 1;
		SpeciesCreationTime = CurrentTimeTick;
	}

	public void CollectData(Godot.Collections.Array<Species.Creature> creaturesInSpecies)
	{
		//update population and fitness data
		PopulationSizeArray.Add((float)creaturesInSpecies.Count);
		CollectMaleFitnessData(creaturesInSpecies);
		//update genetic traits data
		CollectTraitData(SpeedArray, Genome.GeneticTrait.Speed, creaturesInSpecies);
		CollectTraitData(PerceptionArray, Genome.GeneticTrait.Perception, creaturesInSpecies);
		CollectTraitData(MatingCycleArray, Genome.GeneticTrait.MatingCycle, creaturesInSpecies);
		CollectTraitData(HungerResistanceArray, Genome.GeneticTrait.HungerResistance, creaturesInSpecies);
		CollectTraitData(ThirstResistanceArray, Genome.GeneticTrait.ThirstResistance, creaturesInSpecies);
		CollectTraitData(GestationArray, Genome.GeneticTrait.Gestation, creaturesInSpecies);
		CollectTraitData(LitterSizeArray, Genome.GeneticTrait.LitterSize, creaturesInSpecies);
		CollectTraitData(LongevityArray, Genome.GeneticTrait.Longevity, creaturesInSpecies);
		CollectTraitData(IntelligenceArray, Genome.GeneticTrait.Intelligence, creaturesInSpecies);
		CollectTraitData(MemoryArray, Genome.GeneticTrait.Memory, creaturesInSpecies);
		CollectTraitData(StrengthArray, Genome.GeneticTrait.Strength, creaturesInSpecies);
		CollectTraitData(HeatResistanceArray, Genome.GeneticTrait.HeatResistance, creaturesInSpecies);
		CollectTraitData(ColdResistanceArray, Genome.GeneticTrait.ColdResistance, creaturesInSpecies);
		CollectTraitData(StaminaArray, Genome.GeneticTrait.Stamina, creaturesInSpecies);
		CollectTraitData(SleepCycleArray, Genome.GeneticTrait.SleepCycle, creaturesInSpecies);
		//update causes of death data
		CollectCauseOfDeathData(StarvationArray, CurrentStarvationAmount);
		CollectCauseOfDeathData(DehydrationArray, CurrentDehydrationAmount);
		CollectCauseOfDeathData(OldAgeArray, CurrentOldAgeAmount);
		CollectCauseOfDeathData(BeingHuntedArray, CurrentBeingHuntedAmount);
		CollectCauseOfDeathData(HeatStrokeArray, CurrentHeatStrokeAmount);
		CollectCauseOfDeathData(FreezingArray, CurrentFreezingAmount);
		CurrentStarvationAmount = 0;
		CurrentDehydrationAmount = 0;
		CurrentOldAgeAmount = 0;
		CurrentBeingHuntedAmount = 0;
		CurrentHeatStrokeAmount = 0;
		CurrentFreezingAmount = 0;
		//update the current time tick
		CurrentTimeTick++;
	}

	private void CollectMaleFitnessData(Godot.Collections.Array<Species.Creature> creaturesInSpecies)
	{
		float maleFitnessSum = 0;
		int maleFitnessCount = 0;
		for (int i = 0; i < creaturesInSpecies.Count; i++)
		{
			if (creaturesInSpecies[i].MyGender == Species.Gender.Male)
			{
				maleFitnessSum += creaturesInSpecies[i].Fitness;
				maleFitnessCount++;
			}
		}
		if (maleFitnessCount > 0)
			MaleFitness.Add(maleFitnessSum / maleFitnessCount);
		else MaleFitness.Add(0.0f);
	}

	private void CollectCauseOfDeathData(Godot.Collections.Array causeOfDeathArray, int deathsAmount){
		causeOfDeathArray.Add((float)deathsAmount);
		GD.Print(causeOfDeathArray);
	}

	private void CollectTraitData(Godot.Collections.Array traitArray, Genome.GeneticTrait trait, Godot.Collections.Array<Species.Creature> creaturesInSpecies)
	{
		float Sum = 0;
		int Count = 0;
		for (int i = 0; i < creaturesInSpecies.Count; i++)
		{
			Sum += creaturesInSpecies[i].MyGenome.GetTrait(trait);
			Count++;
		}
		if (Count > 0)
			traitArray.Add(Sum / Count);
		else
			traitArray.Add(0.0f);
	}

	public void updateStarvation(){
		CurrentStarvationAmount++;
	}

	public void updateDehydration(){
		CurrentDehydrationAmount++;
	}
	public void updateOldAge(){
		CurrentOldAgeAmount++;
	}

	public void updateBeingHunted(){
		CurrentBeingHuntedAmount++;
	}

	public void updateHeatStroke(){
		CurrentHeatStrokeAmount++;
	}

	public void updateFreezing(){
		CurrentFreezingAmount++;
	}

	public float GetCurrentMaleFitness()
	{
		return (float)MaleFitness[CurrentTimeTick];
	}

	public Godot.Collections.Array GetSpeedData()
	{
		return SpeedArray;
	}
	public Godot.Collections.Array GetPerceptionData()
	{
		return PerceptionArray;
	}
	public Godot.Collections.Array GetMatingCycleData()
	{
		return MatingCycleArray;
	}
	public Godot.Collections.Array GetHungerResistanceData()
	{
		return HungerResistanceArray;
	}
	public Godot.Collections.Array GetThirstResistanceData()
	{
		return ThirstResistanceArray;
	}
	public Godot.Collections.Array GetGestationData()
	{
		return GestationArray;
	}
	public Godot.Collections.Array GetLitterSizeData()
	{
		return LitterSizeArray;
	}
	public Godot.Collections.Array GetLongevityData()
	{
		return LongevityArray;
	}

	public Godot.Collections.Array GetIntelligenceData()
	{
		return IntelligenceArray;
	}
	public Godot.Collections.Array GetMemoryData()
	{
		return MemoryArray;
	}

	public Godot.Collections.Array GetStrengthData()
	{
		return StrengthArray;
	}

	public Godot.Collections.Array GetHeatResistanceData()
	{
		return HeatResistanceArray;
	}

	public Godot.Collections.Array GetColdResistancehData()
	{
		return ColdResistanceArray;
	}

	public Godot.Collections.Array GetStaminaData()
	{
		return StaminaArray;
	}

	public Godot.Collections.Array GetSleepCycleData()
	{
		return SleepCycleArray;
	}

	public Godot.Collections.Array GetPopulationSizeData(){
		return PopulationSizeArray;
	}

	public Godot.Collections.Array GetStarvationData(){
		return StarvationArray;
	}

	public Godot.Collections.Array GetDehydrationData(){
		return DehydrationArray;
	}

	public Godot.Collections.Array GetOldAgeData(){
		return OldAgeArray;
	}

	public Godot.Collections.Array GetBeingHuntedData(){
		return BeingHuntedArray;
	}

	public Godot.Collections.Array GetHeatStrokeData(){
		return HeatStrokeArray;
	}

	public Godot.Collections.Array GetFreezingData(){
		return FreezingArray;
	}


	public int GetSpeciesCreationTime()
	{
		return SpeciesCreationTime;
	}
}
