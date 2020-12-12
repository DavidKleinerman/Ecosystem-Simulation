using Godot;
using System;

public class Species : Spatial
{
	public String SpeciesName;

	private SpatialMaterial SpeciesMaterial;
	private PackedScene CreatureScene = (PackedScene)GD.Load("res://assets/Creature.tscn");

	private RandomNumberGenerator rng;

	private int CurrentTimeTick = 0;

	//Data Arrays
	private Godot.Collections.Array MaleFitness = (Godot.Collections.Array) new Godot.Collections.Array();


	public override void _Ready()
	{
		rng = (RandomNumberGenerator) new RandomNumberGenerator();
	}

	public void SetSpeciesName (String speciesName){
		this.SpeciesName = speciesName;
	}

	public void InitDataArarys(Godot.Collections.Array initArray){
		for (int i = 0; i < initArray.Count; i++){
			MaleFitness.Add(0);
		}
		CurrentTimeTick = initArray.Count - 1;
	}

	public void AddNewCreatures(int popSize, Color color, Godot.Collections.Array initialValues, float geneticVariation){
		SpatialMaterial material = (SpatialMaterial) new SpatialMaterial();
		material.AlbedoColor = color;
		SpeciesMaterial = material;
		foreach (Node n in ReshuffledGroundTiles()){
			Vector3 position = ((Spatial)n).Translation;
			position.y = 5;
			Node newCreatureInst = CreatureScene.Instance();
			Genome genome = new Genome();
			genome.ArtificialCombine(initialValues, geneticVariation);
			((Creature)newCreatureInst).SetGenome(genome);
			((Spatial)newCreatureInst).Translation = position;
			((Creature)newCreatureInst).SetMaterial(material);
			((Creature)newCreatureInst).SpeciesName = SpeciesName;
			AddChild(newCreatureInst);
			popSize--;
			if (popSize == 0)
				break;
		}
	}

	private Godot.Collections.Array ReshuffledGroundTiles(){
		Godot.Collections.Array tilesList = GetTree().GetNodesInGroup("GroundTiles");
		Godot.Collections.Array shuffledList = (Godot.Collections.Array) new Godot.Collections.Array(); 
		Godot.Collections.Array indexList = (Godot.Collections.Array) new Godot.Collections.Array();
		int ListSize = tilesList.Count;
		for (int i = 0; i < ListSize; i++){
			indexList.Add(i);
		}
		for (int i = 0; i < ListSize; i++){
			rng.Randomize();
			int x = rng.RandiRange(0, indexList.Count - 1);
			shuffledList.Add(tilesList[(int)indexList[x]]);
			indexList.Remove(indexList[x]);
		}
		return shuffledList;
	}

	public String GetSpeciesName(){
		return SpeciesName;
	}

	public void AddDead(String cause, Vector3 position){
		GetTree().CallGroup("SpeciesHolder", "AddDead", position);
	}

	public void UpdateTime(){

	}

	public void CollectData(){
		Godot.Collections.Array creaturesInSpecies = GetChildren();
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
		GD.Print(MaleFitness);
		CurrentTimeTick++;
	}

	public float GetCurrentMaleFitness(){
		return (float)MaleFitness[CurrentTimeTick];
	}
}
