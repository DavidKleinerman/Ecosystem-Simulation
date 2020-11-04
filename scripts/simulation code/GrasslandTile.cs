using Godot;
using System;

public class GrasslandTile : GroundTile
{
	private const float grasslandGrowRate = 1.19f;
	protected override void InitializePlantType(){
		PlantType = (PackedScene)GD.Load("res://assets/Plants/GrasslandPlant.tscn");
	}

	protected override float TotalGrowRate(){
		return globalGrowRate * grasslandGrowRate;
	}

	private void _on_Timer_timeout()
	{
		GeneratePlant();
	}
}

