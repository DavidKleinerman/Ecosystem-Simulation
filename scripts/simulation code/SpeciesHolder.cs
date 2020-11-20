using Godot;
using System;

public class SpeciesHolder : Spatial
{
	private int numOfSpecies = 0;

	private PackedScene Species = (PackedScene)GD.Load("res://assets/Species.tscn");
	public override void _Ready()
	{
		
	}

	public void AddSpecies(String speciesName, int popSize, Color color){
		Node newSpeciesInst = Species.Instance();
		AddChild(newSpeciesInst);
		((Species)newSpeciesInst).SetSpeciesName(speciesName);
		((Species)newSpeciesInst).AddNewCreatures(popSize, color);
	}
}
