using Godot;
using System;

public class SpeciesHolder : Spatial
{
	private int numOfSpecies = 0;

	private PackedScene Species = (PackedScene)GD.Load("res://assets/Species.tscn");
	public override void _Ready()
	{
		
	}

	public void AddSpecies(String speciesName, int popSize, Color color, Godot.Collections.Array initialValues, float geneticVariation){
		Node newSpeciesInst = Species.Instance();
		AddChild(newSpeciesInst);
		((Species)newSpeciesInst).SetSpeciesName(speciesName);
		((Species)newSpeciesInst).AddNewCreatures(popSize, color, initialValues, geneticVariation);
	}

	public Godot.Collections.Array GetTraitData(String species, Genome.GeneticTrait trait){
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		Godot.Collections.Array arr = (Godot.Collections.Array) new Godot.Collections.Array();
		for (int i = 0; i < 100; i++){
			rng.Randomize();
			arr.Add(rng.Randf());
		}
		return arr;
	}

	public void AddDead(Vector3 position){
		PackedScene Meat = (PackedScene)GD.Load("res://assets/Meat.tscn");
		Node meatInst = Meat.Instance();
		((Spatial)meatInst).Translation = position;
		AddChild(meatInst);
	}
}
