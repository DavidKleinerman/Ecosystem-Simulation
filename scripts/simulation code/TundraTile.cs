using Godot;
using System;

public class TundraTile : GroundTile
{
	private const float tundraGrowRate = 0.18f;
	protected override void InitializePlantType(){
		PlantType = (PackedScene)GD.Load("res://assets/Plants/TundraPlant.tscn");
	}

	protected override float TotalGrowRate(){
		return globalGrowRate * tundraGrowRate;
	}

	private void _on_Timer_timeout()
	{
		GeneratePlant();
	}
}



