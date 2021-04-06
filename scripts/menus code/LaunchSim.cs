using Godot;
using System;

public class LaunchSim : Button
{
	public enum BiomeTypeSet {
	 	Water = 0,
	 	Forest = 1,
	 	Desert = 2,
	 	Grass = 3,
	 	Tundra = 4
	}
	public enum WorldSizeSet {
		Small = 16,
		Normal = 24,
		Big = 32
	};
	
	private void _on_WorldSizePicker_item_selected(int index)
	{
		GD.Print(index, "\n");
		if(index == 0){
			Global.worldSize = (int)WorldSizeSet.Small;
		}
		else if(index == 1){
			Global.worldSize = (int)WorldSizeSet.Normal;
		}
		else if(index == 2){
			Global.worldSize = (int)WorldSizeSet.Big;
			
		}
	}
	private void _on_BiomeType_item_selected(int index)
	{
		GD.Print(index, "\n");
		if(index == 0){
			Global.biomeType= (int)BiomeTypeSet.Water;
		}
		if(index == 1){
			Global.biomeType= (int)BiomeTypeSet.Forest;
		}
		if(index == 2){
			Global.biomeType= (int)BiomeTypeSet.Desert;
		}
		if(index == 3){
			Global.biomeType= (int)BiomeTypeSet.Grass;
		}
		if(index == 4){
			Global.biomeType= (int)BiomeTypeSet.Tundra;
		}
	}
	private void _on_VSlider_value_changed(float value)
	{
		Global.biomeGrowthRate = value;
		GD.Print(Global.biomeGrowthRate);
	}
	private void _on_LaunchSim_pressed()
	{
		GetTree().ChangeScene("res://assets/Simulation.tscn");
	}
}
