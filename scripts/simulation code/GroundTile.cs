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

	private PackedScene PlantAreaScene = (PackedScene)GD.Load("res://assets/Plants/PlantArea.tscn");

	private PlantArea PlantArea;

	private Godot.Collections.Array Eaters = (Godot.Collections.Array) new Godot.Collections.Array();

	private Godot.Collections.Array FutureEaters = (Godot.Collections.Array) new Godot.Collections.Array();

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

    public override void _PhysicsProcess(float delta)
    {
        if (Eaters.Count > 0){
			Vector3 eatRate = (Vector3) new Vector3(1,1,1);
			eatRate *= Eaters.Count * 0.5f * delta;
			if (((Spatial)plant).Scale.x > 0)
				((Spatial)plant).Scale -= eatRate;
			else {
				if (Eaters.Count > 0){
					foreach (Creature c in Eaters){
						c.StopEating();
						RemoveEater(c);
					}
				}
				if (FutureEaters.Count > 0){
					foreach (Creature c in FutureEaters){
						c.StopEating();
						RemoveFutureEater(c);
					}
				}
				plant.QueueFree();
				PlantArea.QueueFree();
				hasPlant = false;
			}
		}
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
				PlantArea = (PlantArea)PlantAreaScene.Instance();
				PlantArea.SetMyTile(this);
				Vector3 initialScale = (Vector3) new Vector3();
				Vector3 initialPosition = (Vector3) new Vector3();
				initialPosition.y = 1;
				((Spatial)plant).Scale = initialScale;
				((Spatial)plant).Translation = initialPosition;
				AddChild(plant);
				AddChild(PlantArea);
				isPlantGrowing = true;
				hasPlant = true;
				GD.Print("spawned a plant!");
			}
		}
	}

	public void AddEater(Creature eater){
		Eaters.Add(eater);
	}

	public void RemoveEater(Creature eater){
		if (Eaters.Contains(eater))
			Eaters.Remove(eater);
	}

	public void AddFutureEater(Creature eater){
		FutureEaters.Add(eater);
	}

	public void RemoveFutureEater(Creature eater){
		if (FutureEaters.Contains(eater))
			FutureEaters.Remove(eater);
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
