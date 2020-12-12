using Godot;
using System;

public class Creature : KinematicBody
{
	public String SpeciesName;
	private SpatialMaterial SpeciesMaterial;
	private Vector3 Velocity = (Vector3) new Vector3();
	private float GRAV = 9.81f;
	private Timer RotateTimer;
	private Timer StraightTimer;
	private Timer FallingTimer;

	private RandomNumberGenerator RotationRNG = (RandomNumberGenerator) new RandomNumberGenerator();
	private RandomNumberGenerator TimerRNG = (RandomNumberGenerator) new RandomNumberGenerator();
	private RandomNumberGenerator RotateDirectionRNG = (RandomNumberGenerator) new RandomNumberGenerator();

	private float RoatationRate = 0;
	private int RotateDirection = 1;

	private Genome MyGenome;

	public enum State{
		ExploringTheEnvironment,
		GoingToWater,
		Drinking,
		GoingToFood,
		Eating,
		GoingToPotentialPartner
	}

	private State MyState;

	private Spatial CurrentTarget;

	public enum Gender{
		Female,
		Male
	}
	private Gender MyGender;

	private Godot.Collections.Array RejectList = (Godot.Collections.Array) new Godot.Collections.Array();
	private int TopOfRejectList = 0;
	private const int RejectListMaxSize = 5;

	private const float BaseEnergyDecay = 2.5f;
	private const float BaseThirstDecay = 5;
	private const float BaseReproductiveUrgeGrowth = 2;
	private const float MaxGoingToTime = 4;

	private float GoingToTime = 0;

	//*****Traits*****
	private float Speed;
	private float Perception;
	private float MatingCycle;
	private float HungerResistance;
	private float ThirstResistance;
	private float Gestation;

	//Resources
	private float Energy;
	private float Thirst;
	private float ReproductiveUrge;
	private float Age;


	public override void _Ready()
	{
		CurrentTarget = this;
		MyState = State.ExploringTheEnvironment;
		Energy = 100;
		Thirst = 0;
		ReproductiveUrge = 0;
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
		StraightTimer = GetNode<Timer>("StraightTimer");
		FallingTimer = GetNode<Timer>("FallingTimer");
		FallingTimer.OneShot = true;
		FallingTimer.Start();
		RotateTimer.OneShot = true;
		StraightTimer.OneShot = true;
		TimerRNG.Randomize();
		RotateTimer.WaitTime = TimerRNG.RandfRange(0.5f, 2);
		RotateTimer.Start();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (ReproductiveUrge < 100) ReproductiveUrge += ((BaseReproductiveUrgeGrowth + MatingCycle) * delta);
		else ReproductiveUrge = 100;
		if (MyState != State.Eating) Energy -= ((BaseEnergyDecay - HungerResistance) * delta);
		if (MyState != State.Drinking) Thirst += ((BaseThirstDecay - ThirstResistance) * delta);
		if (Energy < 0){
			Die("Starvation");
		}
		if (Thirst > 100){
			Die("Dehydration");
		}
		//GD.Print("Energy: " + Energy + " , Thirst: " + Thirst);
		if(!RotateTimer.IsStopped() && MyState == State.ExploringTheEnvironment){
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
		}
	}

	private void Die(String cause){
		if(MyState == State.GoingToPotentialPartner){
			if (CurrentTarget != null)
				CurrentTarget.GetParent<Creature>().SetState(State.ExploringTheEnvironment);
		} else if (MyState == State.Eating){
			CurrentTarget.GetParent().GetParent<GroundTile>().RemoveEater();
		}
		GetParent<Species>().AddDead(cause, ToGlobal(GetNode<Spatial>("PerceptionRadius").Translation));
		GD.Print(SpeciesName + " Died of " + cause);
		QueueFree();
	}

	public override void _PhysicsProcess(float delta)
	{		
		if (FallingTimer.IsStopped()){
			Vector3 frontVector = ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation) - ToGlobal(GetNode<MeshInstance>("BodyHolder/Body").Translation);
			frontVector.y = 0;
			if(MyState == State.ExploringTheEnvironment)
				Velocity = frontVector.Normalized();
			else if (MyState == State.GoingToWater || MyState == State.GoingToFood || MyState == State.GoingToPotentialPartner)
				GoToTarget(frontVector, delta);
			Velocity *= Speed * delta;
			Velocity.y = -GRAV * 10 * delta;
		} else Velocity.y -= GRAV * delta;
		MoveAndSlide(Velocity, Vector3.Up);
	}

	private void GoToTarget(Vector3 frontVector, float delta){
		GoingToTime += delta;
		if (GoingToTime < MaxGoingToTime){
			Vector3 targetLocation = CurrentTarget.ToGlobal(CurrentTarget.Translation);
			Vector3 myLocation = ToGlobal(GetNode<Spatial>("PerceptionRadius").Translation);
			Vector3 goToTarget = targetLocation - myLocation;
			Velocity = goToTarget.Normalized();
			targetLocation.y = 0;
			if (MyState == State.GoingToFood){
				if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 1.2){
					StopGoingTo(State.Eating);
					CurrentTarget.GetParent().GetParent<GroundTile>().AddEater();
				} else RotateToTarget(targetLocation);
			}
			else if (MyState == State.GoingToWater){
				if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 3){
					StopGoingTo(State.Drinking);
				} else RotateToTarget(targetLocation);
			}
			else if (MyState == State.GoingToPotentialPartner){
				if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 2){
					//change all of this later
					if (RejectList.Count < RejectListMaxSize)
						RejectList.Add(CurrentTarget.GetParent<Creature>());
					else RejectList.Insert(TopOfRejectList, CurrentTarget.GetParent<Creature>());
					TopOfRejectList++;
					if (TopOfRejectList >= RejectListMaxSize)
						TopOfRejectList = 0;
					SetState(State.ExploringTheEnvironment); //change this later
				} else RotateToTarget(targetLocation);
			}
		} else {
			GoingToTime = 0;
			SetState(State.ExploringTheEnvironment);
		}
	}

	private void RotateToTarget(Vector3 targetLocation){
		LookAt(targetLocation, Vector3.Up);
		float myRoatation = Rotation.y;
		Vector3 newRotation = (Vector3) new Vector3(0, myRoatation, 0);
		Rotation = newRotation;

	}

	private void StopGoingTo(State state){
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
		TimerRNG.Randomize();
		StraightTimer.WaitTime = TimerRNG.RandfRange(0.5f, 2);
		StraightTimer.Start();
	}

	private void _on_StraightTimer_timeout()
	{
		RotationRNG.Randomize();
		RoatationRate = RotationRNG.RandfRange(20, 200);
		RotateDirectionRNG.Randomize();
		float generatedRotateValue = RotateDirectionRNG.RandfRange(-1, 1);
		if(generatedRotateValue > 0)
			RotateDirection = 1;
		else RotateDirection = -1;
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
		Speed = 50 + MyGenome.GetTrait(Genome.GeneticTrait.Speed) * 3.5f;
		Perception = MyGenome.GetTrait(Genome.GeneticTrait.Perception) / 20;
		GetNode<Area>("PerceptionRadius").Scale = (Vector3) new Vector3(Perception, 0.2f, Perception);
		MatingCycle = MyGenome.GetTrait(Genome.GeneticTrait.MatingCycle) / 50;
		HungerResistance = MyGenome.GetTrait(Genome.GeneticTrait.HungerResistance) / 50;
		ThirstResistance = MyGenome.GetTrait(Genome.GeneticTrait.ThirstResistance) / 50;
	}

	public void SetState(State state){
		MyState = state;
		if (MyState == State.ExploringTheEnvironment)
			CurrentTarget = this;
	}
	private void _on_PerceptionRadius_body_entered(object body)
	{
		if(body is Node && MyState == State.ExploringTheEnvironment){
			if(((Node)body).IsInGroup("Water")){
				if (Weight() < Thirst * 1.3){
					MyState = State.GoingToWater;
					CurrentTarget = (Spatial)body;
				}
			}
		}
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
				CurrentTarget = creature.GetNode<Spatial>("PerceptionRadius");
				return true;
			}
		}
		return false;
	}

	private void _on_PerceptionRadius_body_exited(object body)
	{

	}

	private void _on_PerceptionRadius_area_entered(object area)
	{
		if (area is Node){
			if(((Node)area).IsInGroup("Plants") && MyState == State.ExploringTheEnvironment){
				if (Weight() < (100 - Energy) * 1.3){
					MyState = State.GoingToFood;
					CurrentTarget = (Spatial)area;
				}
			} else if (((Node)area).GetParent().IsInGroup("Creatures")){
				if (((Node)area).GetParent<Creature>().GetGender() != MyGender && ((Node)area).GetParent<Creature>().SpeciesName == SpeciesName && !RejectList.Contains(((Node)area).GetParent<Creature>())){
					if (Weight() < ReproductiveUrge){
						if (((Node)area).GetParent<Creature>().CheckPotentialPartner(this)){
							MyState = State.GoingToPotentialPartner;
							CurrentTarget = ((Spatial)area);
						}
					}
				}
			}
		}
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

	private void _on_AgeTimer_timeout()
	{
		Die("Old Age");
	}
	
}

