using Godot;
using System;

public class LoadSimButton : Button
{
	public override void _Ready()
	{
		
	}

	private void _on_FileDialog_confirmed()
	{
		try{
			Global.IsLoaded = true;
			String path = GetParent().GetNode<FileDialog>("FileDialog").CurrentPath;
			var saveFile = new File();
			if (!saveFile.FileExists(path)){
				GD.Print("problem in opening file in path: " + path);
				return;
			}
			saveFile.Open(path, File.ModeFlags.Read);
			var loadedData = new Godot.Collections.Array((Godot.Collections.Array)JSON.Parse(saveFile.GetLine()).Result);
			Global.LoadedArray = loadedData;
			GetTree().ChangeScene("res://assets/Simulation.tscn");
		} catch (Exception e) {
			return;
		}
		
	}

}



