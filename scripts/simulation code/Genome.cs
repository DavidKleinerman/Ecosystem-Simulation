using Godot;
using System;

public class Genome
{
	public enum GeneticTrait {
		Speed,
		Perception,
		MatingCycle,
		HungerResistance,
		ThirstResistance,
		Gestation
	}

	public enum ChromosomeSet {
		Maternal,
		Paternal
	}
	private Godot.Collections.Array MaternalChromosomeSet;
	private Godot.Collections.Array PaternalChromosomeSet;
	private Godot.Collections.Array DominanceMask;
	private Godot.Collections.Array TotalGenome;

	Genome(){
		MaternalChromosomeSet = (Godot.Collections.Array) new Godot.Collections.Array();
		PaternalChromosomeSet = (Godot.Collections.Array) new Godot.Collections.Array();
		DominanceMask = (Godot.Collections.Array) new Godot.Collections.Array();
		TotalGenome = (Godot.Collections.Array) new Godot.Collections.Array();

	}

	private void CombineSets(){
		TotalGenome.Insert((int)ChromosomeSet.Maternal, MaternalChromosomeSet);
		TotalGenome.Insert((int)ChromosomeSet.Paternal, PaternalChromosomeSet);
	}

	private void GenerateDominanceMask(){
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		for (int i = 0; i < MaternalChromosomeSet.Count; i++){
			rng.Randomize();
			DominanceMask[i] = rng.RandiRange(0, 1);
		}
	}

	public void ArtificialCombine(Godot.Collections.Array initialValues, float geneticVariation){
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		for (int i = 0; i < initialValues.Count; i++){
			rng.Randomize();
			MaternalChromosomeSet[i] = (float)initialValues[i] + rng.RandfRange(-geneticVariation, geneticVariation);
			rng.Randomize();
			PaternalChromosomeSet[i] = (float)initialValues[i] + rng.RandfRange(-geneticVariation, geneticVariation);
		}
		CombineSets();
		GenerateDominanceMask();
	}

	public void Recombination(Godot.Collections.Array Maternal, Godot.Collections.Array Paternal){
		MaternalChromosomeSet = Maternal;
		PaternalChromosomeSet = Paternal;
		CombineSets();
		GenerateDominanceMask();
	}

	public float GetTrait(GeneticTrait traitIndex){
		return (float)((Godot.Collections.Array)TotalGenome[(int)DominanceMask[(int)traitIndex]])[(int)traitIndex];
	}

	// TO DO
	/*
	public Godot.Collections.Array Meiosis(){
		Godot.Collections.Array crossoveredSet = Crossover();
		return Mutation(crossoveredSet);
	}

	private Godot.Collections.Array CrossOver(){

	}

	private void Mutation(Godot.Collections.Array ChromosomeSet){

	}
	*/
}
