using Godot;
using System;

public class GrasslandTile : MeshInstance
{
	private PackedScene GrasslandPlant = (PackedScene)GD.Load("res://assets/Plants/GrasslandPlant.tscn");
	private bool hasPlant = false;

	private bool isPlantGrowing = false;
	
	private const float grasslandGrowRate = 1.19f;

	private int globalGrowRate = 20;

	private Node plant;

	RandomNumberGenerator rng;

	public override void _Ready()
	{
		rng = (RandomNumberGenerator) new RandomNumberGenerator();
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(isPlantGrowing)
			GrowPlant(delta);
	}

	private void GrowPlant(float delta){
		Vector3 currentScale = ((Spatial)plant).Scale;
		if(currentScale.x < 1){
			currentScale.x += 0.5f * delta;
			currentScale.y += 0.5f * delta;
			currentScale.z += 0.5f * delta;
			((Spatial)plant).Scale = currentScale;
		} else isPlantGrowing = false;

	}

	private void _on_Timer_timeout()
	{
		if(!hasPlant){
			rng.Randomize();
			float plantChance = rng.RandfRange(0f, 100f);
			if (plantChance < grasslandGrowRate * globalGrowRate){
				plant = GrasslandPlant.Instance();
				Vector3 initialScale = (Vector3) new Vector3();
				Vector3 initialPosition = (Vector3) new Vector3();
				initialPosition.y = 1;
				((Spatial)plant).Scale = initialScale;
				((Spatial)plant).Translation = initialPosition;
				AddChild(plant);
				isPlantGrowing = true;
				hasPlant = true;
				GD.Print("spawned a plant!");
			}
		}
	}

	public void StartTimer(){
		GetNode<Timer>("Timer").Start(-1);
	}
}
