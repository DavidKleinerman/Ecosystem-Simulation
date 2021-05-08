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
		Gestation,
		LitterSize,
		Longevity,
		Intelligence,
		Memory,
		Strength,
		HeatResistance,
		ColdResistance,
		Stamina,
		SleepCycle
	}

	public enum ChromosomeSet {
		Maternal,
		Paternal
	}
	private Godot.Collections.Array MaternalChromosomeSet = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array PaternalChromosomeSet = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array DominanceMask = (Godot.Collections.Array) new Godot.Collections.Array();
	private Godot.Collections.Array TotalGenome = (Godot.Collections.Array) new Godot.Collections.Array();

	private const float MinMutationRate = 0.35f;

	public Genome(){

	}

	public Genome(Godot.Collections.Array maternal, Godot.Collections.Array paternal, Godot.Collections.Array dominance){
		for (int i = 0; i < maternal.Count; i++)
			MaternalChromosomeSet.Add((float)maternal[i]);
		for (int i = 0; i < paternal.Count; i++)
			PaternalChromosomeSet.Add((float)paternal[i]);
		for (int i = 0; i < dominance.Count; i++)
			DominanceMask.Add((int)((float)dominance[i]));
		TotalGenome = (Godot.Collections.Array) new Godot.Collections.Array();
		CombineSets();
	}
	public Genome(Godot.Collections.Array maternal, Godot.Collections.Array paternal){
		for (int i = 0; i < maternal.Count; i++)
			MaternalChromosomeSet.Add((float)maternal[i]);
		for (int i = 0; i < paternal.Count; i++)
			PaternalChromosomeSet.Add((float)paternal[i]);
		TotalGenome = (Godot.Collections.Array) new Godot.Collections.Array();
		CombineSets();
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
		if (initialValues.Count == 15){
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
				chromosomeSet[i] = NormalizeValue((float)chromosomeSet[i] + rng.RandfRange(-15, 15));
		}
		return chromosomeSet;
	}

	public Godot.Collections.Array getMaternal(){
		return MaternalChromosomeSet;
	}

	public Godot.Collections.Array getPaternal(){
		return PaternalChromosomeSet;
	}

	public Godot.Collections.Array getDominanceMask(){
		return DominanceMask;
	}

}
