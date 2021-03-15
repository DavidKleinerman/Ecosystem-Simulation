using Godot;
using System;

public class Species : MultiMeshInstance
{
	public String SpeciesName;
	private RandomNumberGenerator rng;
	private DataCollector SpeciesDataCollector = null;
	private Color SpeciesColor;
	private BiomeGrid TileGrid;
	private PackedScene Collider = (PackedScene)GD.Load("res://assets/CreatureCollider.tscn");
	private Area PerceptionCollider;

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
	public class Creature : Godot.Reference { //godot has major bugs when using structs. This is a work-around.
		public string SpeciesName;
		//movement
		public Vector3 Velocity  = (Vector3) new Vector3();
		public Vector3 FrontVector = (Vector3) new Vector3();
		public Spatial MySpatial = new Spatial();
		public CreatureCollider Collider;
		public float CurrentRotationTime = 0;
		public float NextRotationTime = 0;
		public float RotationRate = 0;
		public int RotationDirection = 0;
		public float GoingToTime = 0;
		//genetic and reproduction
		public Genome MyGenome;
		public Gender MyGender;
		public float Fitness;
		public float ReproTime = 0;
		//state and target
		public State MyState = State.ExploringTheEnvironment;
		public Vector3 CurrentTarget = new Vector3();
		public Creature TargetCreature;
		public float ScanInterval;
		public float TimeSinceLastScan = 0;
		//Reject list
		public Godot.Collections.Array RejectList = (Godot.Collections.Array) new Godot.Collections.Array();
		public int TopOfRejectList = 0;
		public int RejectListMaxSize = 5;

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

	private Spatial AssistSpatial;
	private Spatial AssistSpatial2;
	private Godot.Collections.Array<Creature> Creatures = (Godot.Collections.Array<Creature>) new Godot.Collections.Array<Creature>();
	//private Godot.Collections.Array<Creature> CreaturesPhysics = (Godot.Collections.Array<Creature>) new Godot.Collections.Array<Creature>();
	private Godot.Collections.Array DeadArray = (Godot.Collections.Array) new Godot.Collections.Array();

	public override void _Ready()
	{
		PerceptionCollider = GetNode<Area>("PerceptionRadius");
		AssistSpatial = GetNode<Spatial>("AssistSpatial");
		AssistSpatial2 = GetNode<Spatial>("AssistSpatial/AssistSpatial2");
		TileGrid = GetNode<BiomeGrid>("../../BiomeGrid");
		rng = (RandomNumberGenerator) new RandomNumberGenerator();
		Multimesh = new MultiMesh();
		Multimesh.ColorFormat = Godot.MultiMesh.ColorFormatEnum.Float;
		Multimesh.TransformFormat = Godot.MultiMesh.TransformFormatEnum.Transform3d;
		Multimesh.Mesh = (Mesh)GD.Load<Mesh>("res://meshes/CreatureBody.tres");

	}

	// public override void _PhysicsProcess(float delta)
	// {
	// 	Vector3 collisionDetector;
	// 	Vector3 posInGrid;
	// 	for (int i = 0; i < CreaturesPhysics.Count; i++){
	// 		if (CreaturesPhysics[i].MyState == State.ExploringTheEnvironment)
	// 			CreaturesPhysics[i].Velocity = CreaturesPhysics[i].FrontVector;
	// 		// if (Creatures[i].MyState == State.Eating || Creatures[i].MyState == State.Drinking)
	// 		// 	Creatures[i].Velocity = new Vector3();
	// 		if (CreaturesPhysics[i].MyState == State.GoingToWater || CreaturesPhysics[i].MyState == State.GoingToFood || CreaturesPhysics[i].MyState == State.GoingToPotentialPartner)
	// 			GoToTarget(CreaturesPhysics[i], delta);
	// 		CreaturesPhysics[i].Velocity *= CreaturesPhysics[i].Speed * delta;
	// 		// Creatures[i].MySpatial.Translation = new Vector3(Creatures[i].MySpatial.Translation.x + Creatures[i].Velocity.x, 2.4f, Creatures[i].MySpatial.Translation.z + Creatures[i].Velocity.z);
	// 		// Multimesh.SetInstanceTransform(i, Creatures[i].MySpatial.Transform);
	// 		collisionDetector = new Vector3(CreaturesPhysics[i].MySpatial.Translation.x + CreaturesPhysics[i].FrontVector.x, 1, CreaturesPhysics[i].MySpatial.Translation.z + CreaturesPhysics[i].FrontVector.z);
	// 		posInGrid = TileGrid.WorldToMap(collisionDetector);
	// 		if (TileGrid.GetCellItem((int)posInGrid.x, (int)posInGrid.y, (int)posInGrid.z) == 4 || TileGrid.GetCellItem((int)posInGrid.x, (int)posInGrid.y, (int)posInGrid.z) == -1){
	// 			if (TileGrid.GetCellItem((int)posInGrid.x, (int)posInGrid.y, (int)posInGrid.z) == 4 && CreaturesPhysics[i].MyState == State.GoingToWater){
	// 				StopGoingTo(CreaturesPhysics[i], State.Drinking);
	// 			}
	// 			else if (CreaturesPhysics[i].MyState != State.Drinking) {
	// 				CreaturesPhysics[i].MySpatial.RotateY(Mathf.Deg2Rad(180));
	// 				CreaturesPhysics[i].FrontVector = CreaturesPhysics[i].FrontVector.Rotated(Vector3.Up, Mathf.Deg2Rad(180));
	// 				SetState(CreaturesPhysics[i], State.ExploringTheEnvironment);
	// 			}
	// 		}
	// 		CreaturesPhysics[i].MySpatial.Translation = new Vector3(CreaturesPhysics[i].MySpatial.Translation.x + CreaturesPhysics[i].Velocity.x, 2.4f, CreaturesPhysics[i].MySpatial.Translation.z + CreaturesPhysics[i].Velocity.z);
	// 		Multimesh.SetInstanceTransform(i, CreaturesPhysics[i].MySpatial.Transform);
	// 	}
	// }
	private float Weight(){
		rng.Randomize();
		return rng.RandfRange(0,100);
	}

	public override void _Process(float delta)
	{
		Vector3 gridIndex;
		BiomeGrid.GroundTile gt;
		Vector3 collisionDetector;
		Vector3 posInGrid;
		Godot.Collections.Array<Creature> temp = (Godot.Collections.Array<Creature>) new Godot.Collections.Array<Creature>();
		for (int i = 0; i < Creatures.Count; i++){
			if (!DeadArray.Contains(i))
				temp.Add(Creatures[i]);
		}
		Creatures.Clear();
		for (int i=0; i < temp.Count; i++){
			Creatures.Add(temp[i]);
		}
		DeadArray.Clear();
		temp.Clear();
		Multimesh.InstanceCount = Creatures.Count;


		for (int i = 0; i < Creatures.Count; i++){
			// scan environment within perception radius
			Creatures[i].TimeSinceLastScan += delta;
			if (Creatures[i].TimeSinceLastScan > Creatures[i].ScanInterval && Creatures[i].MyState == State.ExploringTheEnvironment){
				ScanEnvironment(Creatures[i]);
			}
			Creatures[i].Age += delta;
			Creatures[i].CurrentRotationTime += delta;
			if (Creatures[i].ReproductiveUrge < 100 && !Creatures[i].Pregnant) Creatures[i].ReproductiveUrge += ((BaseReproductiveUrgeGrowth + Creatures[i].MatingCycle) * delta);
			if (Creatures[i].MyState != State.Eating) Creatures[i].Energy -= ((BaseEnergyDecay - Creatures[i].HungerResistance) * delta);
			if (Creatures[i].MyState != State.Drinking) Creatures[i].Thirst += ((BaseThirstDecay - Creatures[i].ThirstResistance) * delta);
			if (Creatures[i].CurrentRotationTime >= Creatures[i].NextRotationTime){
				rng.Randomize();
				Creatures[i].RotationRate = rng.RandfRange(20, 200);
				rng.Randomize();
				Creatures[i].RotationDirection = rng.RandiRange(-1, 1);
				rng.Randomize();
				Creatures[i].NextRotationTime = rng.RandfRange(0.5f, 2);
				Creatures[i].CurrentRotationTime = 0f;
			}

			if (Creatures[i].Energy < 0){
				Die(Creatures[i], i, CauseOfDeath.Starvation);
			}
			if (Creatures[i].Thirst > 100){
				Die(Creatures[i], i, CauseOfDeath.Dehydration);
			}
			if (Creatures[i].Age > Creatures[i].Longevity){
				Die(Creatures[i], i, CauseOfDeath.OldAge);
			}
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
			else if (Creatures[i].MyState == State.Eating){
				gridIndex = TileGrid.WorldToMap(new Vector3(Creatures[i].CurrentTarget.x, 1, Creatures[i].CurrentTarget.z));
				gt = TileGrid.GetGroundTiles()[gridIndex];
				if (gt.hasPlant){
					Creatures[i].Energy += 25 * delta;
				}
				else {
					gt.EatersCount--;
					SetState(Creatures[i], State.ExploringTheEnvironment);
				}
				if (Creatures[i].Energy > 100){ 
					Creatures[i].Energy = 100;
					gt.EatersCount--;
					SetState(Creatures[i], State.ExploringTheEnvironment);
				}
				
			} else if (Creatures[i].MyState == State.Drinking){
				Creatures[i].Thirst -= 25 * delta;
				if (Creatures[i].Thirst < 0){
					Creatures[i].Thirst = 0;
					SetState(Creatures[i], State.ExploringTheEnvironment);
				} 
			}
			else if (Creatures[i].MyState == State.Reproducing){
				Creatures[i].ReproTime += delta;
				if (Creatures[i].ReproTime > 2){
					Creatures[i].ReproTime = 0;
					Creatures[i].ReproductiveUrge = 0;
					// if (Creatures[i].MyGender == Gender.Female){ //female only
					// 	try{
					// 	Pregnant = true;
					// 	PregnantWithGenome = CurrentTarget.GetParent<Creature>().GetGenome();
					// 	} catch (Exception e) {
					// 		Pregnant = false;
					// 		SetState(State.ExploringTheEnvironment);
					// 	}
					// }
					SetState(Creatures[i], State.ExploringTheEnvironment);
				}
			}






			if (Creatures[i].MyState == State.ExploringTheEnvironment)
				Creatures[i].Velocity = Creatures[i].FrontVector;
			// if (Creatures[i].MyState == State.Eating || Creatures[i].MyState == State.Drinking)
			// 	Creatures[i].Velocity = new Vector3();
			if (Creatures[i].MyState == State.GoingToWater || Creatures[i].MyState == State.GoingToFood || Creatures[i].MyState == State.GoingToPotentialPartner)
				GoToTarget(Creatures[i], delta);
			Creatures[i].Velocity *= Creatures[i].Speed * delta;
			// Creatures[i].MySpatial.Translation = new Vector3(Creatures[i].MySpatial.Translation.x + Creatures[i].Velocity.x, 2.4f, Creatures[i].MySpatial.Translation.z + Creatures[i].Velocity.z);
			// Multimesh.SetInstanceTransform(i, Creatures[i].MySpatial.Transform);
			collisionDetector = new Vector3(Creatures[i].MySpatial.Translation.x + Creatures[i].FrontVector.x, 1, Creatures[i].MySpatial.Translation.z + Creatures[i].FrontVector.z);
			posInGrid = TileGrid.WorldToMap(collisionDetector);
			if (TileGrid.GetCellItem((int)posInGrid.x, (int)posInGrid.y, (int)posInGrid.z) == 4 || TileGrid.GetCellItem((int)posInGrid.x, (int)posInGrid.y, (int)posInGrid.z) == -1){
				if (TileGrid.GetCellItem((int)posInGrid.x, (int)posInGrid.y, (int)posInGrid.z) == 4 && Creatures[i].MyState == State.GoingToWater){
					StopGoingTo(Creatures[i], State.Drinking);
				}
				else if (Creatures[i].MyState != State.Drinking) {
					Creatures[i].MySpatial.RotateY(Mathf.Deg2Rad(180));
					Creatures[i].FrontVector = Creatures[i].FrontVector.Rotated(Vector3.Up, Mathf.Deg2Rad(180));
					SetState(Creatures[i], State.ExploringTheEnvironment);
					if (Creatures[i].MyState == State.GoingToPotentialPartner)
						SetState(Creatures[i].TargetCreature, State.ExploringTheEnvironment);
				}
			}
			Creatures[i].MySpatial.Translation = new Vector3(Creatures[i].MySpatial.Translation.x + Creatures[i].Velocity.x, 2.4f, Creatures[i].MySpatial.Translation.z + Creatures[i].Velocity.z);
			Multimesh.SetInstanceTransform(i, Creatures[i].MySpatial.Transform);
			Multimesh.SetInstanceColor(i, SpeciesColor);
			Creatures[i].Collider.Translation = Creatures[i].MySpatial.Translation;
		}
	}

	private void ScanEnvironment(Creature creature){
		Vector3 gridIndex = TileGrid.WorldToMap(new Vector3(creature.MySpatial.Translation.x, 1, creature.MySpatial.Translation.z));
		rng.Randomize();
		int scanMethod = rng.RandiRange(0,1);
		bool scanForWater = Weight() < creature.Thirst * 1.3f;
		bool scanForFood = Weight() < (100 - creature.Energy) * 1.3f;
		bool scanForReproduction = Weight() < creature.ReproductiveUrge;
		if (scanForFood || scanForWater){
			switch(scanMethod){
				case 0:
					for(int x = (int)(gridIndex.x - creature.Perception); x <= (int)(gridIndex.x + creature.Perception); x++){
						for (int z = (int)(gridIndex.z - creature.Perception); z <= (int)(gridIndex.z + creature.Perception); z++){
							CheckTile(creature, x, z, scanForFood, scanForWater);
							if(creature.MyState != State.ExploringTheEnvironment)
								break;
						}
					}
					break;
				case 1:
					for(int x = (int)(gridIndex.x + creature.Perception); x >= (int)(gridIndex.x - creature.Perception); x--){
						for (int z = (int)(gridIndex.z + creature.Perception); z >= (int)(gridIndex.z - creature.Perception); z--){
							CheckTile(creature, x, z, scanForFood, scanForWater);
							if(creature.MyState != State.ExploringTheEnvironment)
								break;
						}
					}
					break;
			}
		}

		PerceptionCollider.Translation = creature.MySpatial.Translation;
		PerceptionCollider.Scale = new Vector3(2 + (creature.Perception * 4), 0.2f, 2 + (creature.Perception * 4));
		foreach(Node n in PerceptionCollider.GetOverlappingAreas()){
			if (((CreatureCollider)n) != creature.Collider){
				Creature detectedCreature = ((CreatureCollider)n).MyCreature;
				if (scanForReproduction){
					if (detectedCreature.MyGender != creature.MyGender && detectedCreature.SpeciesName == SpeciesName && !creature.RejectList.Contains(detectedCreature)){
						if (CheckPotentialPartner(creature, detectedCreature)){
							creature.MyState = State.GoingToPotentialPartner;
							creature.TargetCreature = detectedCreature;
							break;
						}
					}
				}
			} 
		}
		creature.TimeSinceLastScan = 0;
	}

	private void CheckTile(Creature creature, int x, int z, bool scanForFood, bool scanForWater){
		BiomeGrid.GroundTile gt;
		int cellType = TileGrid.GetCellItem(x, 0, z);
		if(scanForWater && cellType == 4){
			creature.MyState = State.GoingToWater;
			creature.CurrentTarget = TileGrid.MapToWorld(x, 0, z);
		} 
		else if (scanForFood && cellType <= 3 && cellType >= 0){
			gt = TileGrid.GetGroundTiles()[new Vector3(x, 0, z)];
			if (gt.hasPlant){
				creature.MyState = State.GoingToFood;
				creature.CurrentTarget = TileGrid.MapToWorld(x, 0, z);
			}
		}
	}

	private bool CheckPotentialPartner(Creature checkingCreature, Creature checkedCreature){
		if (checkedCreature.MyState != State.ExploringTheEnvironment) return false;
		else if (!checkedCreature.RejectList.Contains(checkingCreature)){
			if (Weight() < checkedCreature.ReproductiveUrge){
				checkedCreature.MyState = State.GoingToPotentialPartner;
				checkedCreature.TargetCreature = checkingCreature;
				return true;
			}
		}
		return false;
	}

	private void Die(Creature creature,int index, CauseOfDeath cause){
		if(creature.MyState == State.GoingToPotentialPartner || creature.MyState == State.Reproducing){
			try {
				SetState(creature.TargetCreature, State.ExploringTheEnvironment);
			}
			catch (Exception e) {
				GD.Print("handeled: \n", e);
			}
		} else if (creature.MyState == State.Eating){
			Vector3 gridIndex = TileGrid.WorldToMap(new Vector3(creature.CurrentTarget.x, 1, creature.CurrentTarget.z));
			BiomeGrid.GroundTile gt = TileGrid.GetGroundTiles()[gridIndex];
			gt.EatersCount--;
		}
		DeadArray.Add(index);
		creature.Collider.QueueFree();
		AddDead(cause, creature.MySpatial.Translation);

	}

	private void GoToTarget(Creature creature, float delta){
		creature.GoingToTime += delta;
		if (creature.GoingToTime < MaxGoingToTime){
			if (creature.MyState == State.GoingToFood){
				Vector3 gridIndex = TileGrid.WorldToMap(new Vector3(creature.CurrentTarget.x, 1, creature.CurrentTarget.z));
				BiomeGrid.GroundTile gt = TileGrid.GetGroundTiles()[gridIndex];
				if (!gt.hasPlant){
					SetState(creature, State.ExploringTheEnvironment);
					creature.GoingToTime = 0;
				}
				else if (creature.MySpatial.Translation.DistanceTo(creature.CurrentTarget) <= 1.8f){
					StopGoingTo(creature, State.Eating);
					gt.EatersCount++;
				} else RotateToTarget(creature, creature.CurrentTarget);
			}
			else if (creature.MyState == State.GoingToWater){
				RotateToTarget(creature, creature.CurrentTarget);
			}
			else if (creature.MyState == State.GoingToPotentialPartner){
				try{
					if (creature.MySpatial.Translation.DistanceTo(creature.TargetCreature.MySpatial.Translation) <= 2){
							if (creature.MyGender == Gender.Female){ //female only
								if(CheckMale(creature.TargetCreature)){
									StopGoingTo(creature, State.Reproducing);
									StopGoingTo(creature.TargetCreature, State.Reproducing);
								} else {
									UpdateRejectList(creature, creature.TargetCreature);
									UpdateRejectList(creature.TargetCreature, creature);
									StopGoingTo(creature, State.ExploringTheEnvironment);
									StopGoingTo(creature.TargetCreature, State.ExploringTheEnvironment);
								}
							}
					} else {
						creature.CurrentTarget = creature.TargetCreature.MySpatial.Translation;
						RotateToTarget(creature, creature.TargetCreature.MySpatial.Translation);
					}
				} catch (Exception e) {
					StopGoingTo(creature, State.ExploringTheEnvironment);
				}
			}
			creature.Velocity = (creature.CurrentTarget - creature.MySpatial.Translation).Normalized();
		} else {
			creature.GoingToTime = 0;
			SetState(creature, State.ExploringTheEnvironment);
		}
	}

	public void UpdateRejectList(Creature listOwner, Creature creature){
		if (listOwner.RejectList.Count < listOwner.RejectListMaxSize)
			listOwner.RejectList.Add(creature);
		else listOwner.RejectList.Insert(listOwner.TopOfRejectList, creature);
		listOwner.TopOfRejectList++;
		if (listOwner.TopOfRejectList >= listOwner.RejectListMaxSize)
			listOwner.TopOfRejectList = 0;
	}

	//female only
	private bool CheckMale(Creature male){
		float AvgFitness = GetCurrentMaleFitness();
		rng.Randomize();
		if(rng.RandfRange(AvgFitness - 0.3f * AvgFitness, AvgFitness + 0.3f * AvgFitness) < male.Fitness)
			return true;
		else return false;
	}

	public void SetState(Creature creature, State state){
		creature.MyState = state;
	}

	public void StopGoingTo(Creature creature, State state){
		creature.GoingToTime = 0;
		SetState(creature, state);
		if (creature.MyState != State.ExploringTheEnvironment)
			creature.Velocity = new Vector3();
	}

	private void RotateToTarget(Creature creature, Vector3 targetLocation){
		AssistSpatial.Transform = creature.MySpatial.Transform;
		AssistSpatial2.Translation = AssistSpatial.ToLocal(creature.MySpatial.Translation + creature.FrontVector);
		AssistSpatial.LookAt(targetLocation, Vector3.Up);
		AssistSpatial.Rotation = new Vector3(0, AssistSpatial.Rotation.y, 0);
		creature.MySpatial.Rotation = AssistSpatial.Rotation;
		creature.FrontVector = (AssistSpatial.ToGlobal(AssistSpatial2.Translation) - AssistSpatial.Translation).Normalized();
		//creature.FrontVector.Rotated(Vector3.Up, angle);
	}

	
	public void InitSpecies (String speciesName, Godot.Collections.Array initArray){
		this.SpeciesName = speciesName;
		SpeciesDataCollector = (DataCollector) new DataCollector(initArray);
	}

	public void AddNewCreatures(int popSize, Color color, Godot.Collections.Array initialValues, float geneticVariation){
		int creatureIndex = 0;
		Multimesh.InstanceCount = popSize;
		SpeciesColor = color;
		foreach (BiomeGrid.GroundTile gt in ReshuffledGroundTiles()){
			Spatial creatureSpatial = new Spatial();
			Vector3 position = TileGrid.MapToWorld((int)gt.gridIndex.x, (int)gt.gridIndex.y, (int)gt.gridIndex.z);
			position.y = 2.4f;
			creatureSpatial.Translation = position;
			Genome genome = new Genome();
			genome.ArtificialCombine(initialValues, geneticVariation);
			AddCreature(genome, creatureSpatial, gt);
			Multimesh.SetInstanceTransform(creatureIndex, creatureSpatial.Transform);
			Multimesh.SetInstanceColor(creatureIndex, SpeciesColor);
			creatureIndex++;
			if (creatureIndex == popSize)
				break;
		}
	}

	private void InitializeTraitsFromGenome(Creature creature){
		creature.Speed = 2 + creature.MyGenome.GetTrait(Genome.GeneticTrait.Speed)/20;
		creature.Perception = creature.MyGenome.GetTrait(Genome.GeneticTrait.Perception) / 25;
		creature.MatingCycle = creature.MyGenome.GetTrait(Genome.GeneticTrait.MatingCycle) / 50;
		creature.HungerResistance = creature.MyGenome.GetTrait(Genome.GeneticTrait.HungerResistance) / 33;
		creature.ThirstResistance = creature.MyGenome.GetTrait(Genome.GeneticTrait.ThirstResistance) / 33;
		creature.Gestation = 6 + creature.MyGenome.GetTrait(Genome.GeneticTrait.Gestation) / 5;
		creature.LitterSize = 1 + Mathf.RoundToInt(creature.MyGenome.GetTrait(Genome.GeneticTrait.LitterSize) / 25);
		creature.Longevity = 20 + creature.MyGenome.GetTrait(Genome.GeneticTrait.Longevity) / 1.25f;
		rng.Randomize();
		creature.ScanInterval = rng.RandfRange(1.8f, 2.2f);
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

	public void AddCreature(Genome genome, Spatial creatureSpatial, BiomeGrid.GroundTile gt){
		Creature creature = new Creature();
		creature.MySpatial = creatureSpatial;
		creature.SpeciesName = SpeciesName;
		creature.MyGenome = genome;
		creature.FrontVector = Vector3.Forward;
		creature.Collider = (CreatureCollider)(Collider.Instance());
		creature.Collider.Translation = creature.MySpatial.Translation;
		creature.Collider.MyCreature = creature;
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
		AddChild(creature.Collider);
	}

	private Godot.Collections.Array<BiomeGrid.GroundTile> ReshuffledGroundTiles(){
		Godot.Collections.Array<BiomeGrid.GroundTile> tilesList = (Godot.Collections.Array<BiomeGrid.GroundTile>) new Godot.Collections.Array<BiomeGrid.GroundTile>(); 
		Godot.Collections.Array<BiomeGrid.GroundTile> shuffledList = (Godot.Collections.Array<BiomeGrid.GroundTile>) new Godot.Collections.Array<BiomeGrid.GroundTile>(); 
		Godot.Collections.Array indexList = (Godot.Collections.Array) new Godot.Collections.Array();
		foreach (Vector3 gridIndex in TileGrid.GetGroundTiles().Keys){
			tilesList.Add(TileGrid.GetGroundTiles()[gridIndex]);
		}
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

	public void AddDead(CauseOfDeath cause, Vector3 position){
		//GetTree().CallGroup("SpeciesHolder", "AddDead", position);
		switch (cause){
			case CauseOfDeath.Starvation:
				SpeciesDataCollector.updateStarvation();
				break;
			case CauseOfDeath.Dehydration:
				SpeciesDataCollector.updateDehydration();
				break;
			case CauseOfDeath.OldAge:
				SpeciesDataCollector.updateOldAge();
				break;
		}
	}

	public void CollectData(){
		// Godot.Collections.Array creaturesInSpecies = GetChildren();
		// if (SpeciesDataCollector != null)
		// 	SpeciesDataCollector.CollectData(creaturesInSpecies);
	}

	public float GetCurrentMaleFitness(){
		return SpeciesDataCollector.GetCurrentMaleFitness();
	}

	public DataCollector GetDataCollector(){
		return SpeciesDataCollector;
	}
}
