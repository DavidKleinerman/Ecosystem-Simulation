using Godot;
using System;

public class Creature : KinematicBody
{
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
		Eating
	}

	private State MyState;

	private Spatial CurrentTarget;

	public enum Gender{
		Female,
		Male
	}
	private Gender MyGender;

	private const float BaseEnergyDecay = 3;
	private const float BaseThirstDecay = 5;

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


	public override void _Ready()
	{
		CurrentTarget = this;
		MyState = State.ExploringTheEnvironment;
		Energy = 100;
		Thirst = 0;
		RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
		rng.Randomize();
		if (rng.RandiRange(0, 1) == 0)
			MyGender = Gender.Female;
		else MyGender = Gender.Male;
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
		if (MyState != State.Eating) Energy -= ((BaseEnergyDecay - HungerResistance) * delta);
		if (MyState != State.Drinking) Thirst += ((BaseThirstDecay - ThirstResistance) * delta);
		if (Energy < 0) Energy = 0;
		if (Thirst > 100) Thirst = 100;
		//GD.Print("Energy: " + Energy + " , Thirst: " + Thirst);
		if(!RotateTimer.IsStopped() && MyState == State.ExploringTheEnvironment){
			RotateY(Mathf.Deg2Rad(RoatationRate * RotateDirection * delta));
		} else if (MyState == State.Eating){
			Energy += 25 * delta;
			if (Energy > 100){ 
				Energy = 100;
				CurrentTarget.GetParent().GetParent<GroundTile>().RemoveEater();
				SetStateToExploring();
			}
		} else if (MyState == State.Drinking){
			Thirst -= 25 * delta;
			if (Thirst < 0){
				Thirst = 0;
				SetStateToExploring();
			} 
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		
		if (FallingTimer.IsStopped()){
			Vector3 frontVector = ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation) - ToGlobal(GetNode<MeshInstance>("BodyHolder/Body").Translation);
			frontVector.y = 0;
			if(MyState == State.ExploringTheEnvironment)
				Velocity = frontVector.Normalized();
			else if (MyState == State.GoingToWater || MyState == State.GoingToFood) GoToTarget(frontVector, delta);
			Velocity *= Speed * delta;
			Velocity.y = -GRAV * 10 * delta;
		} else Velocity.y -= GRAV * delta;
		MoveAndSlide(Velocity, Vector3.Up);
	}

	private void GoToTarget(Vector3 frontVector, float delta){
		
		if (ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation).DistanceTo(CurrentTarget.ToGlobal(CurrentTarget.Translation)) <= 1.2){
			Velocity = (Vector3) new Vector3();
			if (MyState == State.GoingToFood){
				MyState = State.Eating;
				CurrentTarget.GetParent().GetParent<GroundTile>().AddEater();
			}
			else if (MyState == State.GoingToWater)
				MyState = State.Drinking;
		}
		else{
			Vector3 plantLocation = CurrentTarget.ToGlobal(CurrentTarget.Translation);
			Vector3 myLocation = ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation);
			Vector3 goToTarget = plantLocation - myLocation;
			Velocity = goToTarget.Normalized();
			RotateY(frontVector.AngleTo(goToTarget) * delta);
		}
	} 

	public void SetMaterial(SpatialMaterial material){
		SpeciesMaterial = material;
		GetNode<MeshInstance>("BodyHolder/Body").SetSurfaceMaterial(0, material);
		GetNode<MeshInstance>("BodyHolder/Head").SetSurfaceMaterial(0, material);
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
		HungerResistance = MyGenome.GetTrait(Genome.GeneticTrait.HungerResistance) / 50;
		ThirstResistance = MyGenome.GetTrait(Genome.GeneticTrait.ThirstResistance) / 50;
	}

	private void SetStateToExploring(){
		MyState = State.ExploringTheEnvironment;
		CurrentTarget = this;
	}
	private void _on_PerceptionRadius_body_entered(object body)
	{

	}

	private void _on_PerceptionRadius_body_exited(object body)
	{
		
	}

	private void _on_PerceptionRadius_area_entered(object area)
	{
		if (area is Node){
			if(((Node)area).IsInGroup("Plants") && MyState == State.ExploringTheEnvironment){
				RandomNumberGenerator rng = (RandomNumberGenerator) new RandomNumberGenerator();
				rng.Randomize();
				float weight = rng.RandfRange(0,100);
				if (weight < 100 - Energy){
					MyState = State.GoingToFood;
					CurrentTarget = (Spatial)area;
				}
			}
		}
	}

	private void _on_PerceptionRadius_area_exited(object area)
	{
		if (area is Spatial){
			if ((Spatial)area == CurrentTarget && MyState == State.Eating){
				CurrentTarget.GetParent().GetParent<GroundTile>().RemoveEater();
				SetStateToExploring();
			}
		}
	}
	
}



