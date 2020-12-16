using Godot;
using System;

public class DataCollector
{
	private int CurrentTimeTick = 0;
	private Godot.Collections.Array MaleFitness = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array SpeedArray = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array PerceptionArray = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array MatingCycleArray = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array HungerResistanceArray = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array ThirstResistanceArray = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array GestationArray = (Godot.Collections.Array) new Godot.Collections.Array();

	public DataCollector(Godot.Collections.Array initArray){
		for (int i = 0; i < initArray.Count; i++){
			MaleFitness.Add(0);
			SpeedArray.Add(0);
			PerceptionArray.Add(0);
			MatingCycleArray.Add(0);
			HungerResistanceArray.Add(0);
			ThirstResistanceArray.Add(0);
			GestationArray.Add(0);
		}
		CurrentTimeTick = initArray.Count - 1;
	}

	public void CollectData(Godot.Collections.Array creaturesInSpecies){
		CollectMaleFitnessData(creaturesInSpecies);
		CollectTraitData(SpeedArray, Genome.GeneticTrait.Speed, creaturesInSpecies);
		CollectTraitData(PerceptionArray, Genome.GeneticTrait.Perception, creaturesInSpecies);
		CollectTraitData(MatingCycleArray, Genome.GeneticTrait.MatingCycle, creaturesInSpecies);
		CollectTraitData(HungerResistanceArray, Genome.GeneticTrait.HungerResistance, creaturesInSpecies);
		CollectTraitData(ThirstResistanceArray, Genome.GeneticTrait.ThirstResistance, creaturesInSpecies);
		CollectTraitData(GestationArray, Genome.GeneticTrait.Gestation, creaturesInSpecies);
		CurrentTimeTick++;
	}

	private void CollectMaleFitnessData(Godot.Collections.Array creaturesInSpecies){
		float maleFitnessSum = 0;
		int maleFitnessCount = 0;
		for(int i = 0; i < creaturesInSpecies.Count; i++){
			if (((Creature)creaturesInSpecies[i]).GetGender() == Creature.Gender.Male){
				maleFitnessSum += ((Creature)creaturesInSpecies[i]).GetFitness();
				maleFitnessCount++;
			}
		}
		if (maleFitnessCount > 0)
			MaleFitness.Add(maleFitnessSum/maleFitnessCount);
		else MaleFitness.Add(0);
		GD.Print("male fitness: ", MaleFitness);
	}

	private void CollectTraitData(Godot.Collections.Array traitArray, Genome.GeneticTrait trait, Godot.Collections.Array creaturesInSpecies){
		float Sum = 0;
		int Count = 0;
		for (int i = 0; i < creaturesInSpecies.Count; i++){
			Sum += ((Creature)creaturesInSpecies[i]).GetGenome().GetTrait(trait);
			Count++;
		}
		if (Count > 0)
			traitArray.Add(Sum/Count);
		else 
			traitArray.Add(0);
		GD.Print(trait, ": ", traitArray);
	}

	public float GetCurrentMaleFitness(){
		return (float)MaleFitness[CurrentTimeTick];
	}

	public Godot.Collections.Array GetSpeedData(){
		return SpeedArray;
	}
	public Godot.Collections.Array GetPerceptionData(){
		return PerceptionArray;
	}
	public Godot.Collections.Array GetMatingCycleData(){
		return MatingCycleArray;
	}
	public Godot.Collections.Array GetHungetResistanceData(){
		return HungerResistanceArray;
	}
	public Godot.Collections.Array GetThirstResistanceData(){
		return ThirstResistanceArray;
	}
	public Godot.Collections.Array GetGestationData(){
		return GestationArray;
	}
}
