using Godot;
using System;

public class Species : Spatial
{
	public String speciesName;

	private SpatialMaterial SpeciesMaterial;
	private PackedScene Creature = (PackedScene)GD.Load("res://assets/Creature.tscn");

	private RandomNumberGenerator rng;
	public override void _Ready()
	{
		rng = (RandomNumberGenerator) new RandomNumberGenerator();
	}

	public void SetSpeciesName (String speciesName){
		this.speciesName = speciesName;
	}

	public void AddNewCreatures(int popSize, Color color){
		SpatialMaterial material = (SpatialMaterial) new SpatialMaterial();
		material.AlbedoColor = color;
		SpeciesMaterial = material;
		foreach (Node n in ReshuffledGroundTiles()){
			Vector3 position = ((Spatial)n).Translation;
			position.y = 5;
			Node newCreatureInst = Creature.Instance();
			((Spatial)newCreatureInst).Translation = position;
			((Creature)newCreatureInst).SetMaterial(material);
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
}
