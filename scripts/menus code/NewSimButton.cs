using Godot;
using System;

public class NewSimButton : Button
{
	

	private void _on_NewSimButton_pressed()
	{
		GetTree().ChangeScene("res://assets/NewSimMenu.tscn");
	}

}

