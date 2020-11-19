using Godot;
using System;

public class Species : Spatial
{
	public String speciesName;

	private PackedScene Creature = (PackedScene)GD.Load("res://assets/Creature.tscn");
	public override void _Ready()
	{
		
	}

	public void SetSpeciesName (String speciesName){
		this.speciesName = speciesName;
	}

	public void AddNewCreatures(int popSize){
		foreach (Node n in GetTree().GetNodesInGroup("GroundTiles")){
			Vector3 position = ((Spatial)n).Translation;
			position.y = 3;
			Node newCreatureInst = Creature.Instance();
			((Spatial)newCreatureInst).Translation = position;
			Vector3 scale = (Vector3) new Vector3(0.5f, 0.5f, 0.5f);
			((Spatial)newCreatureInst).Scale = scale;
			AddChild(newCreatureInst);
			popSize--;
			if (popSize == 0)
				break;
		}
	}
}
