using Godot;
using System;

public class SettingsMenu : Control
{
	public override void _Ready()
	{
		this.Visible = true;
	}
	



	private void _on_ItemList_item_selected(int index)
	{
		GD.Print(index, "\n");
		if(index == 1){
			//OS.SetWindowMaximized(false);
			OS.SetBorderlessWindow(true);
			OS.SetWindowSize(OS.GetScreenSize());
		}
		else if(index == 0){
			OS.SetBorderlessWindow(false);
			//OS.SetWindowMaximized(true);
		}
	}
	
}
