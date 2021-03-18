using Godot;
using System;

public class PauseMenu : Control
{
	public override void _Ready()
	{
		this.Visible = false;
	}
	private void _on_Resume_pressed()
	{
		this.Visible = false;
	}
	private void _on_BackToMainMenu_pressed()
	{
		GetTree().ChangeScene("res://assets/MainMenu.tscn");
	}
	private void _on_Exit_pressed()
	{
		GetTree().Quit();
	}
	private void _on_Pause_pressed()
	{
		this.Visible = true;
	}
}
