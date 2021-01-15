using Godot;
using System;

public class Creature : Area
{
	public String SpeciesName;
	private SpatialMaterial SpeciesMaterial;
	private Vector3 Velocity = (Vector3) new Vector3();
	private Timer RotateTimer;
	private bool IsColliding = false;

	private RandomNumberGenerator RotationRNG = (RandomNumberGenerator) new RandomNumberGenerator();
	private RandomNumberGenerator TimerRNG = (RandomNumberGenerator) new RandomNumberGenerator();
	private RandomNumberGenerator RotateDirectionRNG = (RandomNumberGenerator) new RandomNumberGenerator();

	private float RoatationRate = 0;
	private int RotateDirection = 1;

	private Genome MyGenome;

	private float Fitness = 0;

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

	private State MyState;

	private Spatial CurrentTarget;

	private float ReproTime = 0;

	public enum Gender{
		Female,
		Male
	}
	private Gender MyGender;

	//Reject list
	private Godot.Collections.Array RejectList = (Godot.Collections.Array) new Godot.Collections.Array();
	private int TopOfRejectList = 0;
	private const int RejectListMaxSize = 5;

	//consts
	private const float BaseEnergyDecay = 3.5f;
	private const float BaseThirstDecay = 4f;
	private const float BaseReproductiveUrgeGrowth = 2.5f;
	private const float MaxGoingToTime = 4;

	private float GoingToTime = 0;

	//*****Traits*****
	private float Speed;
	private float Perception;
	private float MatingCycle;
	private float HungerResistance;
	private float ThirstResistance;
	private float Gestation;
	private int LitterSize;
	private float Longevity;

	//Resources
	private float Energy;
	private float Thirst;
	private float ReproductiveUrge;
	private float Age;

	// Passive states
	private bool Pregnant = false;
	private bool Growing = false;

	//female only
	private Genome PregnantWithGenome = null;
	private float PregnancyTime = 0;
	private float PreviousPregnancyTime = 0;
	private int BornChildren = 0;
	private const int TimeToBirth = 3;
	private float BirthingTime = 0;


	public override void _Ready()
	{
		CurrentTarget = this;
		MyState = State.ExploringTheEnvironment;
		Energy = 100;
		Thirst = 0;
		ReproductiveUrge = 0;
		Age = 0;
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		rng.Randomize();
		if (rng.RandiRange(0, 1) == 0){
			MyGender = Gender.Female;
			GetNode<MeshInstance>("BodyHolder/Head").SetSurfaceMaterial(0, GD.Load<SpatialMaterial>("res://materials/PinkCreature_material.tres"));
		}
		else {
			MyGender = Gender.Male;
			GetNode<MeshInstance>("BodyHolder/Head").SetSurfaceMaterial(0, GD.Load<SpatialMaterial>("res://materials/BlueCreature_material.tres"));
		}
		RotateTimer = GetNode<Timer>("RotateTimer");
		RotateTimer.OneShot = true;
		TimerRNG.Randomize();
		RotateTimer.WaitTime = TimerRNG.RandfRange(0.5f, 2);
		RotateTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Age += delta;
		if (ReproductiveUrge < 100 && !Pregnant) ReproductiveUrge += ((BaseReproductiveUrgeGrowth + MatingCycle) * delta);
		if (MyState != State.Eating) Energy -= ((BaseEnergyDecay - HungerResistance) * delta);
		if (MyState != State.Drinking) Thirst += ((BaseThirstDecay - ThirstResistance) * delta);
		if (Energy < 0){
			Die("Starvation");
		}
		if (Thirst > 100){
			Die("Dehydration");
		}
		if (Age > Longevity){
			Die("Old Age");
		}
		if (Pregnant){ //female only
			PregnancyTime += delta;
			if (PregnancyTime >= Gestation){
				StartBirthingProcess();
			}
			else if (PregnancyTime >= Gestation * 0.8f && PreviousPregnancyTime < Gestation * 0.8f){
				if (Weight() < 50){
					StartBirthingProcess();
				}
			}
			else if (PregnancyTime >= Gestation * 0.6f && PreviousPregnancyTime < Gestation * 0.6f){
				if (Weight() < 25){
					StartBirthingProcess();
				}
			}
			PreviousPregnancyTime = PregnancyTime;
		}
		if (MyState == State.GivingBirth){
			BirthingProcess(delta);
		}
		if(MyState == State.ExploringTheEnvironment){
			RotateY(Mathf.Deg2Rad(RoatationRate * RotateDirection * delta));
		} else if (MyState == State.Eating){
			Energy += 25 * delta;
			if (Energy > 100){ 
				Energy = 100;
				CurrentTarget.GetParent().GetParent<GroundTile>().RemoveEater();
				SetState(State.ExploringTheEnvironment);
			}
		} else if (MyState == State.Drinking){
			Thirst -= 25 * delta;
			if (Thirst < 0){
				Thirst = 0;
				SetState(State.ExploringTheEnvironment);
			} 
		} else if (MyState == State.Reproducing){
			ReproTime += delta;
			if (ReproTime > 2){
				ReproTime = 0;
				ReproductiveUrge = 0;
				if (MyGender == Gender.Female){ //female only
					try{
					Pregnant = true;
					PregnantWithGenome = CurrentTarget.GetParent<Creature>().GetGenome();
					} catch (Exception e) {
						Pregnant = false;
						SetState(State.ExploringTheEnvironment);
					}
				}
				SetState(State.ExploringTheEnvironment);
			}
		}
	}

	private void GiveBirth(){
		Godot.Collections.Array paternal = PregnantWithGenome.Meiosis();
		Godot.Collections.Array maternal = MyGenome.Meiosis();
		Genome genome = new Genome();
		genome.Recombination(maternal, paternal);
		GetParent<Species>().AddCreature(genome, ToGlobal(GetNode<Spatial>("PerceptionRadius").Translation), SpeciesMaterial);
	}

	private void BirthingProcess(float delta){
		BirthingTime += delta;
		if (BirthingTime > TimeToBirth){
			GiveBirth();
			BirthingTime = 0;
			BornChildren++;
			if (BornChildren == LitterSize){
				BornChildren = 0;
				PregnantWithGenome = null;
				SetState(State.ExploringTheEnvironment);
			}
		}
	}

	private void StartBirthingProcess(){
		PregnancyTime = 0;
		PreviousPregnancyTime = 0;
		Pregnant = false;
		CurrentTarget = this;
		SetState(State.GivingBirth);
		Velocity = new Vector3();
	}

	private void Die(String cause){
		if(MyState == State.GoingToPotentialPartner || MyState == State.Reproducing){
			try {
				CurrentTarget.GetParent<Creature>().SetState(State.ExploringTheEnvironment);
			}
			catch (Exception e) {
				GD.Print("handeled: \n", e);
			}
		} else if (MyState == State.Eating){
			CurrentTarget.GetParent().GetParent<GroundTile>().RemoveEater();
		}
		GetParent<Species>().AddDead(cause, ToGlobal(GetNode<Spatial>("PerceptionRadius").Translation));
		GD.Print(SpeciesName + " Died of " + cause);
		QueueFree();
	}

	public override void _PhysicsProcess(float delta)
	{
		if (MyState == State.ExploringTheEnvironment){
			Vector3 frontVector = ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation) - ToGlobal(GetNode<MeshInstance>("BodyHolder/Body").Translation);
			Velocity = frontVector.Normalized();
		}
		if (MyState == State.GoingToWater || MyState == State.GoingToFood || MyState == State.GoingToPotentialPartner)
			GoToTarget(delta);
		Velocity *= Speed * delta;
		Translation = new Vector3(Translation.x + Velocity.x, 1, Translation.z + Velocity.z);
		if (IsColliding)
			IsStillColliding();
	}

	private void GoToTarget(float delta){
		GoingToTime += delta;
		Vector3 targetLocation;
		if (GoingToTime < MaxGoingToTime){
			try {
				targetLocation = CurrentTarget.ToGlobal(CurrentTarget.Translation);
			} catch (Exception e) {
				StopGoingTo(State.ExploringTheEnvironment);
				return;
			}
			Vector3 myLocation = ToGlobal(GetNode<Spatial>("PerceptionRadius").Translation);
			Vector3 goToTarget = targetLocation - myLocation;
			Velocity = goToTarget.Normalized();
			if (MyState == State.GoingToFood){
				if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 1.2){
					StopGoingTo(State.Eating);
					CurrentTarget.GetParent().GetParent<GroundTile>().AddEater();
				} else RotateToTarget(targetLocation);
			}
			else if (MyState == State.GoingToWater){
				if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 3.8){
					StopGoingTo(State.Drinking);
				} else RotateToTarget(targetLocation);
			}
			else if (MyState == State.GoingToPotentialPartner){
				try{
					if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 2){
						if (CurrentTarget != null){
							if (MyGender == Gender.Female){ //female only
								if(CheckMale()){
									StopGoingTo(State.Reproducing);
									CurrentTarget.GetParent<Creature>().StopGoingTo(State.Reproducing);
								} else {
									CurrentTarget.GetParent<Creature>().UpdateRejectList(this);
									UpdateRejectList(CurrentTarget.GetParent<Creature>());
									CurrentTarget.GetParent<Creature>().StopGoingTo(State.ExploringTheEnvironment);
									StopGoingTo(State.ExploringTheEnvironment);
								}
							}
						}
					} else RotateToTarget(targetLocation);
				} catch (Exception e) {
					StopGoingTo(State.ExploringTheEnvironment);
				}
			}
		} else {
			GoingToTime = 0;
			SetState(State.ExploringTheEnvironment);
		}
	}

	public void UpdateRejectList(Creature creature){
		if (RejectList.Count < RejectListMaxSize)
			RejectList.Add(creature);
		else RejectList.Insert(TopOfRejectList, creature);
		TopOfRejectList++;
		if (TopOfRejectList >= RejectListMaxSize)
			TopOfRejectList = 0;
	}

	//female only
	private bool CheckMale(){
		float targetFitness = CurrentTarget.GetParent<Creature>().GetFitness();
		float AvgFitness = GetParent<Species>().GetCurrentMaleFitness();
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		rng.Randomize();
		if(rng.RandfRange(AvgFitness - 0.3f * AvgFitness, AvgFitness + 0.3f * AvgFitness) < targetFitness)
			return true;
		else return false;
	}

	private void RotateToTarget(Vector3 targetLocation){
		LookAt(targetLocation, Vector3.Up);
		float myRoatation = Rotation.y;
		Rotation = (Vector3) new Vector3(0, myRoatation, 0);
	}

	public void StopGoingTo(State state){
		GoingToTime = 0;
		SetState(state);
		if (MyState != State.ExploringTheEnvironment)
			Velocity = (Vector3) new Vector3();
	}

	public void SetMaterial(SpatialMaterial material){
		SpeciesMaterial = material;
		GetNode<MeshInstance>("BodyHolder/Body").SetSurfaceMaterial(0, material);
	}

	private void _on_RotateTimer_timeout()
	{
		RotationRNG.Randomize();
		RoatationRate = RotationRNG.RandfRange(20, 200);
		RotateDirectionRNG.Randomize();
		RotateDirection = RotateDirectionRNG.RandiRange(-1, 1);
		TimerRNG.Randomize();
		RotateTimer.WaitTime = TimerRNG.RandfRange(0.5f, 2);
		RotateTimer.Start();
	}

	public void SetGenome(Genome genome){
		MyGenome = genome;
		InitializeTraitsFromGenome();
	}

	public Genome GetGenome(){
		return MyGenome;
	}

	private void InitializeTraitsFromGenome(){
		Speed = 2 + MyGenome.GetTrait(Genome.GeneticTrait.Speed)/20;
		Perception = MyGenome.GetTrait(Genome.GeneticTrait.Perception) / 20;
		GetNode<Area>("PerceptionRadius").Scale = (Vector3) new Vector3(Perception, 0.2f, Perception);
		MatingCycle = MyGenome.GetTrait(Genome.GeneticTrait.MatingCycle) / 50;
		HungerResistance = MyGenome.GetTrait(Genome.GeneticTrait.HungerResistance) / 33;
		ThirstResistance = MyGenome.GetTrait(Genome.GeneticTrait.ThirstResistance) / 33;
		Gestation = 6 + MyGenome.GetTrait(Genome.GeneticTrait.Gestation) / 5;
		LitterSize = 1 + Mathf.RoundToInt(MyGenome.GetTrait(Genome.GeneticTrait.LitterSize) / 25);
		Longevity = 20 + MyGenome.GetTrait(Genome.GeneticTrait.Longevity) / 1.25f;
		CalcFitness();
	}

	private void CalcFitness(){
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.Speed);
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.Perception);
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.MatingCycle);
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.HungerResistance);
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.ThirstResistance);
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.Gestation);
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.LitterSize);
		Fitness += MyGenome.GetTrait(Genome.GeneticTrait.Longevity);
	}

	public float GetFitness(){
		return Fitness;
	}

	public void SetState(State state){
		MyState = state;
		if (MyState == State.ExploringTheEnvironment)
			CurrentTarget = this;
	}

	private float Weight(){
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		rng.Randomize();
		return rng.RandfRange(0,100);
	}

	public bool CheckPotentialPartner(Creature creature){
		if (MyState != State.ExploringTheEnvironment) return false;
		else if (!RejectList.Contains(creature)){
			if (Weight() < ReproductiveUrge){
				MyState = State.GoingToPotentialPartner;
				CurrentTarget = creature.GetNode<Spatial>("PerceptionRadius");;
				return true;
			}
		}
		return false;
	}

	private void _on_PerceptionRadius_area_entered(object area)
	{
		if (area is Node && MyState == State.ExploringTheEnvironment){
			if(((Node)area).IsInGroup("Plants")){
				if (Weight() < (100 - Energy) * 1.3){
					MyState = State.GoingToFood;
					CurrentTarget = (Spatial)area;
				}
			} else if (((Node)area).IsInGroup("CreatureRadius")){
				Creature detectedCreature = ((Node)area).GetParent<Creature>();
				if (detectedCreature.GetGender() != MyGender && detectedCreature.SpeciesName == SpeciesName && !RejectList.Contains(detectedCreature)){
					if (Weight() < ReproductiveUrge){
						if (detectedCreature.CheckPotentialPartner(this)){
							MyState = State.GoingToPotentialPartner;
							CurrentTarget = ((Spatial)area);
						}
					}
				}
			} else if (((Node)area).IsInGroup("Water")){
				if (Weight() < Thirst * 1.3){
					MyState = State.GoingToWater;
					CurrentTarget = (Spatial)area;
				}	
			}
		}
	}

	private void _on_Creature_area_entered(object area)
	{
		if (area is Node){
			if (((Node)area).IsInGroup("Walls")){
				RotateY(Mathf.Deg2Rad(180));
				IsColliding = true;
				if(MyState == State.GoingToPotentialPartner || MyState == State.Reproducing){
					try {
						CurrentTarget.GetParent<Creature>().SetState(State.ExploringTheEnvironment);
					}
					catch (Exception e) {
						GD.Print("handeled: \n", e);
					}
				}
				SetState(State.ExploringTheEnvironment);
				GoingToTime = 0;
				MoveAwayFromCollider();
			}
		}

	}

	private void MoveAwayFromCollider(){
		Vector3 frontVector = ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation) - ToGlobal(GetNode<MeshInstance>("BodyHolder/Body").Translation);
		Velocity = frontVector.Normalized();
		Velocity *= 0.1f;
		Translation = new Vector3(Translation.x + Velocity.x, 1, Translation.z + Velocity.z);
	}


	private void IsStillColliding(){
		Vector3 allCollisions = (Vector3) new Vector3();
		int numOfCollisions = 0;
		foreach (Node n in GetOverlappingAreas()){
			if (n.IsInGroup("Walls")){
				allCollisions += ((Area)n).ToGlobal(((Area)n).Translation);
				numOfCollisions++;
			}
		}
		if (numOfCollisions > 0){
			RotateToTarget(allCollisions / numOfCollisions);
			RotateY(Mathf.Deg2Rad(180));
			MoveAwayFromCollider();
		} else IsColliding = false;
	}

	private void _on_PerceptionRadius_area_exited(object area)
	{
		if (area is Spatial){
			if ((Spatial)area == CurrentTarget){
				if (MyState == State.Eating || MyState == State.GoingToFood){
					if (MyState == State.Eating)
						CurrentTarget.GetParent().GetParent<GroundTile>().RemoveEater();
					SetState(State.ExploringTheEnvironment);
				}
			}
		}
	}

	public Creature.Gender GetGender(){
		return MyGender;
	}
	
}
