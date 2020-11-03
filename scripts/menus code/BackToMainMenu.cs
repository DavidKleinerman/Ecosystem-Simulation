using Godot;
using System;

public class BackToMainMenu : Button
{
	private void _on_BackToMainMenu_pressed()
	{
		GetTree().ChangeScene("res://assets/MainMenu.tscn");
	}
}



