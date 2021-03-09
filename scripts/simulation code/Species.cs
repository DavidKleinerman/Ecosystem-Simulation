using Godot;
using System;

public class Species : MultiMeshInstance
{
	public String SpeciesName;
	//private PackedScene CreatureScene = (PackedScene)GD.Load("res://assets/Creature.tscn");
	private RandomNumberGenerator rng;
	private DataCollector SpeciesDataCollector = null;

	//consts
	private const float BaseEnergyDecay = 3.5f;
	private const float BaseThirstDecay = 4f;
	private const float BaseReproductiveUrgeGrowth = 2.5f;
	private const float MaxGoingToTime = 4;

	//enums
	public enum State{
		ExploringTheEnvironment,
		GoingToWater,
		Drinking,
		GoingToFood,
		Eating,
		GoingToPotentialPartner,
		Reproducing,
		GivingBirth
	}

	public enum CauseOfDeath {
		Starvation,
		Dehydration,
		OldAge
	}
	public enum Gender{
		Female,
		Male
	}
	public class Creature : Godot.Object { //godot has major bugs when using structs. This is a work-around.
		//movement
		public Vector3 Velocity;
		public Spatial MySpatial;
		public float RoatateTimer;
		public float RatationRate;
		public int RotationDiretion;
		public bool IsColliding;
		private float GoingToTime = 0;
		//genetic and reproduction
		public Genome MyGenome;
		private Gender MyGender;
		public float Fitness;
		//state and target
		public State MyState;
		public Spatial CurrentTarget;
		
		//Reject list
		public Godot.Collections.Array RejectList = (Godot.Collections.Array) new Godot.Collections.Array();
		public int TopOfRejectList = 0;
		public const int RejectListMaxSize = 5;

		//*****Traits*****
		public float Speed;
		public float Perception;
		public float MatingCycle;
		public float HungerResistance;
		public float ThirstResistance;
		public float Gestation;
		public int LitterSize;
		public float Longevity;

		//Resources
		public float Energy;
		public float Thirst;
		public float ReproductiveUrge;
		public float Age;

		// Passive states
		public bool Pregnant = false;
		public bool Growing = false;

		//female only
		public Genome PregnantWithGenome = null;
		public float PregnancyTime = 0;
		public float PreviousPregnancyTime = 0;
		public int BornChildren = 0;
		public const int TimeToBirth = 3;
		public float BirthingTime = 0;
	}

	private Godot.Collections.Array<Creature> Creatures = (Godot.Collections.Array<Creature>) new Godot.Collections.Array<Creature>();

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
			Genome genome = new Genome();
			genome.ArtificialCombine(initialValues, geneticVariation);
			Creature creature = new Creature();
			creature.MySpatial = creatureSpatial;
			creature.MyGenome = genome;
			Creatures.Add(creature);
			Multimesh.SetInstanceTransform(creatureIndex, creatureSpatial.Transform);
			Multimesh.SetInstanceColor(creatureIndex, color);
			creatureIndex++;
			if (creatureIndex == popSize)
				break;
		}
	}

	// public void AddCreature(Genome genome, Vector3 position, SpatialMaterial material){
	// 	Node newCreatureInst = CreatureScene.Instance();
	// 	((Creature)newCreatureInst).SetGenome(genome);
	// 	((Spatial)newCreatureInst).Translation = position;
	// 	((Creature)newCreatureInst).SetMaterial(material);
	// 	((Creature)newCreatureInst).SpeciesName = SpeciesName;
	// 	AddChild(newCreatureInst);
	// }

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

	// public void AddDead(Creature.CauseOfDeath cause, Vector3 position){
	// 	GetTree().CallGroup("SpeciesHolder", "AddDead", position);
	// 	switch (cause){
	// 		case Creature.CauseOfDeath.Starvation:
	// 			SpeciesDataCollector.updateStarvation();
	// 			break;
	// 		case Creature.CauseOfDeath.Dehydration:
	// 			SpeciesDataCollector.updateDehydration();
	// 			break;
	// 		case Creature.CauseOfDeath.OldAge:
	// 			SpeciesDataCollector.updateOldAge();
	// 			break;
	// 	}
	// }

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
