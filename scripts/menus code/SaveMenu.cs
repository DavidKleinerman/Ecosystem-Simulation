using Godot;
using System;

public class SaveMenu : Control
{

	public override void _Ready()
	{
		
	}

	private void _on_SaveSimulation_pressed()
	{
		GetNode<FileDialog>("FileDialog").Popup_();
		GetParent().GetNode<Control>("PauseMenu").Visible = false;
	}
	private void _on_FileDialog_popup_hide()
	{
		GetParent().GetNode<Control>("PauseMenu").Visible = true;
	}

}






