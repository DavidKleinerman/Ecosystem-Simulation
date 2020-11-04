using Godot;
using System;

public class ForestTile : GroundTile
{
	private const float forestGrowRate = 17.92f;
	protected override void InitializePlantType(){
		PlantType = (PackedScene)GD.Load("res://assets/Plants/ForestPlant.tscn");
	}

	protected override float TotalGrowRate(){
		return globalGrowRate * forestGrowRate;
	}
	private void _on_Timer_timeout()
	{
		GeneratePlant();
	}

}


