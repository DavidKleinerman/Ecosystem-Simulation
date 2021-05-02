using Godot;
using System;

public class SaveMenu : Control
{

	private void _on_SaveSimulation_pressed()
	{
		GetNode<FileDialog>("FileDialog").Popup_();
		GetParent().GetNode<Control>("PauseMenu").Visible = false;
	}
	private void _on_FileDialog_popup_hide()
	{
		GetParent().GetNode<Control>("PauseMenu").Visible = true;
	}
	
	private void _on_FileDialog_confirmed()
	{
		String path = GetNode<FileDialog>("FileDialog").CurrentPath;
		path += ".save";
		var saveFile = new File();
		saveFile.Open(path, File.ModeFlags.Write);
		Godot.Collections.Array savedTiles = GetParent().GetNode<BiomeGrid>("BiomeGrid").Save();
		saveFile.StoreLine(JSON.Print(savedTiles));
	}

}









