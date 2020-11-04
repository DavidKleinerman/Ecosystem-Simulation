using Godot;
using System;

public class DesertTile : GroundTile
{
	private const float desertGrowRate = 0.57f;
	protected override void InitializePlantType(){
		PlantType = (PackedScene)GD.Load("res://assets/Plants/DesertPlant.tscn");
	}

	protected override float TotalGrowRate(){
		return globalGrowRate * desertGrowRate;
	}
	
	private void _on_Timer_timeout()
	{
		GeneratePlant();
	}
}

