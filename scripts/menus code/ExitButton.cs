using Godot;
using System;

public class ExitButton : Button
{
	
	private void _on_ExitButton_pressed()
	{
		GetTree().Quit();
	}

}



