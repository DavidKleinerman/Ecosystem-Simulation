using Godot;
using System;

public class Meat : Spatial
{
	private void _on_Timer_timeout()
	{
		QueueFree();
	}

}


