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

	private const float MinMutationRate = 0.1f;

	public Genome(){
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
		DominanceMask.Clear();
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		for (int i = 0; i < MaternalChromosomeSet.Count; i++){
			rng.Randomize();
			DominanceMask.Add(rng.RandiRange(0, 1));
		}
	}

	public void ArtificialCombine(Godot.Collections.Array initialValues, float geneticVariation){
		if (initialValues.Count == 6){
			MaternalChromosomeSet.Clear();
			PaternalChromosomeSet.Clear();
		} else return;
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		for (int i = 0; i < initialValues.Count; i++){
			rng.Randomize();
			MaternalChromosomeSet.Add(NormalizeValue((float)initialValues[i] + rng.RandfRange(-geneticVariation, geneticVariation)));
			rng.Randomize();
			PaternalChromosomeSet.Add(NormalizeValue((float)initialValues[i] + rng.RandfRange(-geneticVariation, geneticVariation)));
		}
		CombineSets();
		GenerateDominanceMask();
	}

	private float NormalizeValue(float value){
		if (value > 100)
			value = 100;
		else if (value < 0)
			value = 0;
		return value;
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

	
	public Godot.Collections.Array Meiosis(){
		Godot.Collections.Array crossoveredSet = CrossOver();
		return Mutation(crossoveredSet);
	}

	private Godot.Collections.Array CrossOver(){
		Godot.Collections.Array newSet = (Godot.Collections.Array) new Godot.Collections.Array();
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		for (int i = 0; i < MaternalChromosomeSet.Count; i++){
			rng.Randomize();
			newSet.Add(((Godot.Collections.Array)TotalGenome[rng.RandiRange(0,1)])[i]);
		}
		return newSet;
	}

	private Godot.Collections.Array Mutation(Godot.Collections.Array chromosomeSet){
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		for (int i = 0; i < chromosomeSet.Count; i++){
			rng.Randomize();
			if (rng.RandfRange(0,1) < MinMutationRate)
				chromosomeSet[i] = (float)chromosomeSet[i] + rng.RandfRange(-5, 5);
		}
		return chromosomeSet;
	}

}
