using Godot;
using System;

public class LaunchSim : Button
{
	

	private void _on_LaunchSim_pressed()
	{
		GetTree().ChangeScene("res://assets/Simulation.tscn");
	}
}



