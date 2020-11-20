using Godot;
using System;

public class GroundTile : Spatial
{
    protected PackedScene PlantType;
	private bool hasPlant = false;

	private bool isPlantGrowing = false;
	
	protected const int globalGrowRate = 5;

	private Node plant;

	RandomNumberGenerator rng;

    public override void _Ready()
	{
		rng = (RandomNumberGenerator) new RandomNumberGenerator();
        InitializePlantType();
	}

    protected virtual void InitializePlantType(){}
	
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

	protected void GeneratePlant()
	{
		if(!hasPlant){
			rng.Randomize();
			float plantChance = rng.RandfRange(0f, 100f);
			if (plantChance < TotalGrowRate()){
				plant = PlantType.Instance();
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

    protected virtual float TotalGrowRate(){ return globalGrowRate;}

	public void StartTimer(){
        GeneratePlant();
		GetNode<Timer>("Timer").Start(-1);
	}

	public void RemoveCollider(){
		GetNode<StaticBody>("StaticBody").QueueFree();
	}
}
