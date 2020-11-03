using Godot;
using System;

public class SettingButton : Button
{
	
	private void _on_SettingButton_pressed()
	{
		GetTree().ChangeScene("res://assets/SettingsMenu.tscn");
	}
}



