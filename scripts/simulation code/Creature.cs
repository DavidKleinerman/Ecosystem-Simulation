using Godot;
using System;

public class Creature : KinematicBody
{
	private SpatialMaterial SpeciesMaterial;
	private Vector3 velocity = (Vector3) new Vector3();
	private float GRAV = 9.81f;

	private float Speed = 200;

	private Timer RotateTimer;

	private Timer StraightTimer;
	private Timer FallingTimer;

	private RandomNumberGenerator RotationRNG = (RandomNumberGenerator) new RandomNumberGenerator();

	private RandomNumberGenerator TimerRNG = (RandomNumberGenerator) new RandomNumberGenerator();

	private RandomNumberGenerator RotateDirectionRNG = (RandomNumberGenerator) new RandomNumberGenerator();

	private float RoatationRate = 0;

	private int RotateDirection = 1;

	
	public override void _Ready()
	{
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
		if(!RotateTimer.IsStopped()){
			RotateY(Mathf.Deg2Rad(RoatationRate * RotateDirection * delta));
		}
		
	}

	public override void _PhysicsProcess(float delta)
	{
		if (FallingTimer.IsStopped()){
			Vector3 frontVector = ToGlobal(GetNode<MeshInstance>("BodyHolder/Head").Translation) - ToGlobal(GetNode<MeshInstance>("BodyHolder/Body").Translation);
			frontVector.y = 0;
			velocity = frontVector.Normalized() * Speed * delta;
		} else velocity.y -= GRAV * delta;
		MoveAndSlide(velocity, Vector3.Up);
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

}









