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
		public Vector3 Velocity  = (Vector3) new Vector3();
		public Vector3 FrontVector = (Vector3) new Vector3();
		public Spatial MySpatial = new Spatial();
		public float CurrentRotationTime = 0;
		public float NextRotationTime = 0;
		public float RotationRate = 0;
		public int RotationDirection = 0;
		public bool IsColliding = false;
		public float GoingToTime = 0;
		//genetic and reproduction
		public Genome MyGenome;
		public Gender MyGender;
		public float Fitness;
		//state and target
		public State MyState = State.ExploringTheEnvironment;
		public Spatial CurrentTarget = null;
		
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
		public float Energy = 100;
		public float Thirst = 0;
		public float ReproductiveUrge = 0;
		public float Age = 0;

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

	public override void _PhysicsProcess(float delta)
	{
		for (int i = 0; i < Creatures.Count; i++){
			if (Creatures[i].MyState == State.ExploringTheEnvironment){
				Creatures[i].Velocity = Creatures[i].FrontVector.Normalized();
			}
			// if (Creatures[i].MyState == State.GoingToWater || Creatures[i].MyState == State.GoingToFood || Creatures[i].MyState == State.GoingToPotentialPartner)
			// 	GoToTarget(delta);
			Creatures[i].Velocity *= Creatures[i].Speed * delta;
			Creatures[i].MySpatial.Translation = new Vector3(Creatures[i].MySpatial.Translation.x + Creatures[i].Velocity.x, 2.4f, Creatures[i].MySpatial.Translation.z + Creatures[i].Velocity.z);
			Multimesh.SetInstanceTransform(i, Creatures[i].MySpatial.Transform);
			// if (Creatures[i].IsColliding)
			// 	IsStillColliding();
		}
	}
	public override void _Process(float delta)
	{
		for (int i = 0; i < Creatures.Count; i++){
			Creatures[i].Age += delta;
			Creatures[i].CurrentRotationTime += delta;
			if (Creatures[i].ReproductiveUrge < 100 && !Creatures[i].Pregnant) Creatures[i].ReproductiveUrge += ((BaseReproductiveUrgeGrowth + Creatures[i].MatingCycle) * delta);
			if (Creatures[i].MyState != State.Eating) Creatures[i].Energy -= ((BaseEnergyDecay - Creatures[i].HungerResistance) * delta);
			if (Creatures[i].MyState != State.Drinking) Creatures[i].Thirst += ((BaseThirstDecay - Creatures[i].ThirstResistance) * delta);
			if (Creatures[i].CurrentRotationTime >= Creatures[i].NextRotationTime){
				RandomNumberGenerator rng = new RandomNumberGenerator();
				rng.Randomize();
				Creatures[i].RotationRate = rng.RandfRange(20, 200);
				rng.Randomize();
				Creatures[i].RotationDirection = rng.RandiRange(-1, 1);
				rng.Randomize();
				Creatures[i].NextRotationTime = rng.RandfRange(0.5f, 2);
				Creatures[i].CurrentRotationTime = 0f;
			}
			// if (Creatures[i].Energy < 0){
			// 	Die(Creature.CauseOfDeath.Starvation);
			// }
			// if (Creatures[i].Thirst > 100){
			// 	Die(Creature.CauseOfDeath.Dehydration);
			// }
			// if (Creatures[i].Age > Longevity){
			// 	Die(Creature.CauseOfDeath.OldAge);
			// }
			// if (Creatures[i].Pregnant){ //female only
			// 	Creatures[i].PregnancyTime += delta;
			// 	if (Creatures[i].PregnancyTime >= Creatures[i].Gestation){
			// 		StartBirthingProcess();
			// 	}
			// 	else if (Creatures[i].PregnancyTime >= Creatures[i].Gestation * 0.8f && Creatures[i].PreviousPregnancyTime < Creatures[i].Gestation * 0.8f){
			// 		if (Weight() < 50){
			// 			StartBirthingProcess();
			// 		}
			// 	}
			// 	else if (Creatures[i].PregnancyTime >= Creatures[i].Gestation * 0.6f && Creatures[i].PreviousPregnancyTime < Creatures[i].Gestation * 0.6f){
			// 		if (Weight() < 25){
			// 			StartBirthingProcess();
			// 		}
			// 	}
			// 	Creatures[i].PreviousPregnancyTime = Creatures[i].PregnancyTime;
			// }
			// if (Creatures[i].MyState == State.GivingBirth){
			// 	BirthingProcess(delta);
			// }
			if(Creatures[i].MyState == State.ExploringTheEnvironment){
				Creatures[i].MySpatial.RotateY(Mathf.Deg2Rad(Creatures[i].RotationRate * Creatures[i].RotationDirection * delta));
				Creatures[i].FrontVector = Creatures[i].FrontVector.Rotated(Vector3.Up, Mathf.Deg2Rad(Creatures[i].RotationRate * Creatures[i].RotationDirection * delta));
			}
			Multimesh.SetInstanceTransform(i, Creatures[i].MySpatial.Transform);
			// else if (MyState == State.Eating){
			// 	Energy += 25 * delta;
			// 	if (Energy > 100){ 
			// 		Energy = 100;
			// 		CurrentTarget.GetParent().GetParent<GroundTile>().RemoveEater();
			// 		SetState(State.ExploringTheEnvironment);
			// 	}
			// } else if (MyState == State.Drinking){
			// 	Thirst -= 25 * delta;
			// 	if (Thirst < 0){
			// 		Thirst = 0;
			// 		SetState(State.ExploringTheEnvironment);
			// 	} 
			// } else if (MyState == State.Reproducing){
			// 	ReproTime += delta;
			// 	if (ReproTime > 2){
			// 		ReproTime = 0;
			// 		ReproductiveUrge = 0;
			// 		if (MyGender == Gender.Female){ //female only
			// 			try{
			// 			Pregnant = true;
			// 			PregnantWithGenome = CurrentTarget.GetParent<Creature>().GetGenome();
			// 			} catch (Exception e) {
			// 				Pregnant = false;
			// 				SetState(State.ExploringTheEnvironment);
			// 			}
			// 		}
			// 		SetState(State.ExploringTheEnvironment);
			// 	}
			// }
		}
	}

	
	// private void GoToTarget(float delta){
	// 	GoingToTime += delta;
	// 	Vector3 targetLocation;
	// 	if (GoingToTime < MaxGoingToTime){
	// 		try {
	// 			targetLocation = CurrentTarget.ToGlobal(CurrentTarget.Translation);
	// 		} catch (Exception e) {
	// 			StopGoingTo(State.ExploringTheEnvironment);
	// 			return;
	// 		}
	// 		Vector3 myLocation = ToGlobal(GetNode<Spatial>("PerceptionRadius").Translation);
	// 		Vector3 goToTarget = targetLocation - myLocation;
	// 		Velocity = goToTarget.Normalized();
	// 		if (MyState == State.GoingToFood){
	// 			if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 1.2){
	// 				StopGoingTo(State.Eating);
	// 				CurrentTarget.GetParent().GetParent<GroundTile>().AddEater();
	// 			} else RotateToTarget(targetLocation);
	// 		}
	// 		else if (MyState == State.GoingToWater){
	// 			if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 3.8){
	// 				StopGoingTo(State.Drinking);
	// 			} else RotateToTarget(targetLocation);
	// 		}
	// 		else if (MyState == State.GoingToPotentialPartner){
	// 			try{
	// 				if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 2){
	// 					if (CurrentTarget != null){
	// 						if (MyGender == Gender.Female){ //female only
	// 							if(CheckMale()){
	// 								StopGoingTo(State.Reproducing);
	// 								CurrentTarget.GetParent<Creature>().StopGoingTo(State.Reproducing);
	// 							} else {
	// 								CurrentTarget.GetParent<Creature>().UpdateRejectList(this);
	// 								UpdateRejectList(CurrentTarget.GetParent<Creature>());
	// 								CurrentTarget.GetParent<Creature>().StopGoingTo(State.ExploringTheEnvironment);
	// 								StopGoingTo(State.ExploringTheEnvironment);
	// 							}
	// 						}
	// 					}
	// 				} else RotateToTarget(targetLocation);
	// 			} catch (Exception e) {
	// 				StopGoingTo(State.ExploringTheEnvironment);
	// 			}
	// 		}
	// 	} else {
	// 		GoingToTime = 0;
	// 		SetState(State.ExploringTheEnvironment);
	// 	}
	// }

	
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
			creature.FrontVector = Vector3.Back;
			RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
			rng.Randomize();
			if (rng.RandiRange(0, 1) == 0){
				creature.MyGender = Gender.Female;
			}
			else {
				creature.MyGender = Gender.Male;
			}
			rng.Randomize();
			creature.NextRotationTime = rng.RandfRange(0.5f, 2);
			InitializeTraitsFromGenome(creature);
			Creatures.Add(creature);
			Multimesh.SetInstanceTransform(creatureIndex, creatureSpatial.Transform);
			Multimesh.SetInstanceColor(creatureIndex, color);
			creatureIndex++;
			if (creatureIndex == popSize)
				break;
		}
	}

	private void InitializeTraitsFromGenome(Creature creature){
		creature.Speed = 2 + creature.MyGenome.GetTrait(Genome.GeneticTrait.Speed)/20;
		creature.Perception = creature.MyGenome.GetTrait(Genome.GeneticTrait.Perception) / 20;
		//GetNode<Area>("PerceptionRadius").Scale = (Vector3) new Vector3(creature.Perception, 0.2f, creature.Perception);
		creature.MatingCycle = creature.MyGenome.GetTrait(Genome.GeneticTrait.MatingCycle) / 50;
		creature.HungerResistance = creature.MyGenome.GetTrait(Genome.GeneticTrait.HungerResistance) / 33;
		creature.ThirstResistance = creature.MyGenome.GetTrait(Genome.GeneticTrait.ThirstResistance) / 33;
		creature.Gestation = 6 + creature.MyGenome.GetTrait(Genome.GeneticTrait.Gestation) / 5;
		creature.LitterSize = 1 + Mathf.RoundToInt(creature.MyGenome.GetTrait(Genome.GeneticTrait.LitterSize) / 25);
		creature.Longevity = 20 + creature.MyGenome.GetTrait(Genome.GeneticTrait.Longevity) / 1.25f;
		CalcFitness(creature);
	}

	private void CalcFitness(Creature creature){
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.Speed);
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.Perception);
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.MatingCycle);
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.HungerResistance);
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.ThirstResistance);
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.Gestation);
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.LitterSize);
		creature.Fitness += creature.MyGenome.GetTrait(Genome.GeneticTrait.Longevity);
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
