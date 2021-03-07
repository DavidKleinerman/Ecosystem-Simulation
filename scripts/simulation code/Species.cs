using Godot;
using System;

public class Species : MultiMeshInstance
{
	public String SpeciesName;

	private SpatialMaterial SpeciesMaterial;
	private PackedScene CreatureScene = (PackedScene)GD.Load("res://assets/Creature.tscn");

	private RandomNumberGenerator rng;

	private DataCollector SpeciesDataCollector = null;


	public override void _Ready()
	{
		rng = (RandomNumberGenerator) new RandomNumberGenerator();
		Multimesh = new MultiMesh();
		Multimesh.ColorFormat = Godot.MultiMesh.ColorFormatEnum.Float;
		Multimesh.TransformFormat = Godot.MultiMesh.TransformFormatEnum.Transform3d;
		Multimesh.Mesh = (Mesh)GD.Load<Mesh>("res://meshes/CreatureBody.tres");

	}

	public void InitSpecies (String speciesName, Godot.Collections.Array initArray){
		this.SpeciesName = speciesName;
		SpeciesDataCollector = (DataCollector) new DataCollector(initArray);
	}

	public void AddNewCreatures(int popSize, Color color, Godot.Collections.Array initialValues, float geneticVariation){
		BiomeGrid biomeGrid = GetNode<BiomeGrid>("../../BiomeGrid");
		int creatureIndex = 0;
		Multimesh.InstanceCount = popSize;
		foreach (BiomeGrid.GroundTile gt in ReshuffledGroundTiles(biomeGrid.GetGroundTiles())){
			Spatial creatureSpatial = new Spatial();
			Vector3 position = biomeGrid.MapToWorld((int)gt.gridIndex.x, (int)gt.gridIndex.y, (int)gt.gridIndex.z);
			position.y = 2.4f;
			creatureSpatial.Translation = position;
			//Genome genome = new Genome();
			//genome.ArtificialCombine(initialValues, geneticVariation);
			//AddCreature(genome, position, material);
			Multimesh.SetInstanceTransform(creatureIndex, creatureSpatial.Transform);
			Multimesh.SetInstanceColor(creatureIndex, color);
			creatureIndex++;
			if (creatureIndex == popSize)
				break;
		}
	}

	public void AddCreature(Genome genome, Vector3 position, SpatialMaterial material){
		Node newCreatureInst = CreatureScene.Instance();
		((Creature)newCreatureInst).SetGenome(genome);
		((Spatial)newCreatureInst).Translation = position;
		((Creature)newCreatureInst).SetMaterial(material);
		((Creature)newCreatureInst).SpeciesName = SpeciesName;
		AddChild(newCreatureInst);
	}

	private Godot.Collections.Array<BiomeGrid.GroundTile> ReshuffledGroundTiles(Godot.Collections.Array<BiomeGrid.GroundTile> tilesList){
		Godot.Collections.Array<BiomeGrid.GroundTile> shuffledList = (Godot.Collections.Array<BiomeGrid.GroundTile>) new Godot.Collections.Array<BiomeGrid.GroundTile>(); 
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

	public void AddDead(Creature.CauseOfDeath cause, Vector3 position){
		GetTree().CallGroup("SpeciesHolder", "AddDead", position);
		switch (cause){
			case Creature.CauseOfDeath.Starvation:
				SpeciesDataCollector.updateStarvation();
				break;
			case Creature.CauseOfDeath.Dehydration:
				SpeciesDataCollector.updateDehydration();
				break;
			case Creature.CauseOfDeath.OldAge:
				SpeciesDataCollector.updateOldAge();
				break;
		}
	}

	public void CollectData(){
		Godot.Collections.Array creaturesInSpecies = GetChildren();
		if (SpeciesDataCollector != null)
			SpeciesDataCollector.CollectData(creaturesInSpecies);
	}

	public float GetCurrentMaleFitness(){
		return SpeciesDataCollector.GetCurrentMaleFitness();
	}

	public DataCollector GetDataCollector(){
		return SpeciesDataCollector;
	}
}
